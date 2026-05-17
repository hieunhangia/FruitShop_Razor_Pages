using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Repository;
using Repository.Models.Products;
using Service.DTOs.Customer.Chatbot;

namespace Service.Customer;

public class ChatbotService(
    [FromKeyedServices("GeminiMainChatService")]
    IChatCompletionService mainChatCompletionService,
    [FromKeyedServices("GeminiSubChatService")]
    IChatCompletionService subChatCompletionService,
    AppDbContext context,
    VectorStoreService vectorStoreService)
{
    private readonly ChatHistoryTruncationReducer reducer = new(BusinessRuleConstants.Chatbot.MaxChatHistoryMessages);

    public async Task ResetVectorStoreAsync()
    {
        await vectorStoreService.DeleteAllFromVectorStoreAsync();
        var products = await context.Products.AsNoTracking()
            .Include(product => product.Categories)
            .Include(product => product.ProductUnit)
            .ToListAsync();
        await vectorStoreService.SaveToVectorStoreAsync(products.Select(p =>
        {
            var content = GetProductContent(p);
            return new VectorDataModel
            {
                Id = p.Id.ToString(),
                Content = content,
                ContentEmbedding = content
            };
        }).ToList());
        return;

        string GetProductContent(Product product)
        {
            var sb = new StringBuilder(
                $"Tên sản phẩm: {product.Name}. Mô tả sản phẩm: {product.Description}. Link sản phẩm: /product/{product.Id}. ");

            if (product.Categories!.Count != 0)
            {
                sb.Append(
                    $"Sản phẩm này thuộc các danh mục: {string.Join(", ", product.Categories.Select(c => c.Name))}. ");
            }

            var unitName = product.ProductUnit!.Name;
            sb.Append($"Giá niêm yết: {product.Price:N0} VNĐ mỗi {unitName}. ");
            if (product.IsActive)
            {
                sb.Append("Tình trạng kinh doanh: Đang được bày bán. ");
                if (product.Quantity > 0)
                {
                    sb.Append(
                        $"Tình trạng tồn kho: Còn hàng. Tổng số lượng còn trong kho là: {product.Quantity} {unitName}. ");
                }
                else
                {
                    sb.Append("Tình trạng tồn kho: Hết hàng. ");
                }
            }
            else
            {
                sb.Append("Tình trạng kinh doanh: Tạm ngừng kinh doanh. ");
            }

            return sb.ToString();
        }
    }

    public async Task<string> Ask(List<ChatMessageDto> messages, string message)
    {
        var chatHistory = new ChatHistory(BusinessRuleConstants.Chatbot.SystemPrompt);
        chatHistory.AddRange(messages.Select(m => new ChatMessageContent(m.Role, m.Content)).ToList());
        var reducedChatHistory = await reducer.ReduceAsync(chatHistory);
        if (reducedChatHistory != null)
        {
            chatHistory = new ChatHistory(reducedChatHistory);
        }

        var tempChatHistory = new ChatHistory(chatHistory);
        tempChatHistory.AddUserMessage(BusinessRuleConstants.Chatbot.IntentClassificationPrompt(message));
        var questionType = (await subChatCompletionService.GetChatMessageContentAsync(tempChatHistory)).Content?.Trim();
        return questionType switch
        {
            BusinessRuleConstants.Chatbot.GeneralQuestionType => await AskGeneralQuestionAsync(chatHistory, message),
            BusinessRuleConstants.Chatbot.RAGQuestionType => await AskRAGQuestionAsync(chatHistory, message),
            _ => BusinessRuleConstants.Chatbot.CannotAnswerQuestionResponse
        };
    }

    private async Task<string> AskGeneralQuestionAsync(ChatHistory chatHistory, string message)
    {
        chatHistory.AddUserMessage(BusinessRuleConstants.Chatbot.GeneralQuestionPrompt(message));
        return (await subChatCompletionService.GetChatMessageContentAsync(chatHistory)).Content ?? string.Empty;
    }

    private async Task<string> AskRAGQuestionAsync(ChatHistory chatHistory, string message)
    {
        chatHistory.AddUserMessage(
            BusinessRuleConstants.Chatbot.RAGQuestionPrompt(message,
                string.Join("\n", await vectorStoreService.SearchFromVectorStoreAsync(message))));
        return (await mainChatCompletionService.GetChatMessageContentAsync(chatHistory)).Content ?? string.Empty;
    }
}