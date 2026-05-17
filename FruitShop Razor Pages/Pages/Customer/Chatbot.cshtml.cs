using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SemanticKernel.ChatCompletion;
using Service.Customer;
using Service.DTOs.Customer.Chatbot;

namespace FruitShop_Razor_Pages.Pages.Customer;

public class ChatbotModel(ChatbotService service, ILogger<ChatbotModel> logger) : PageModel
{
    private const string SessionKeyChatHistory = "ChatHistory";
    public List<ChatMessageDto> ChatHistory { get; set; } = [];

    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Vui lòng nhập câu hỏi của bạn.")]
        public string Message { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        ChatHistory = HttpContext.Session.GetObject<List<ChatMessageDto>>(SessionKeyChatHistory) ?? [];
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ChatHistory = HttpContext.Session.GetObject<List<ChatMessageDto>>(SessionKeyChatHistory) ?? [];
            var response = await service.Ask(ChatHistory, Input.Message);
            ChatHistory.Add(new ChatMessageDto { Role = AuthorRole.User, Content = Input.Message });
            ChatHistory.Add(new ChatMessageDto { Role = AuthorRole.Assistant, Content = response });
            HttpContext.Session.SetObject(SessionKeyChatHistory, ChatHistory);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while processing chatbot question.");
            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xử lý câu hỏi của bạn. Vui lòng thử lại sau.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPostNewSession()
    {
        HttpContext.Session.Remove(SessionKeyChatHistory);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResetVectorStoreAsync()
    {
        await service.ResetVectorStoreAsync();
        TempData["SuccessMessage"] = "Đã reset vector store thành công.";

        return RedirectToPage();
    }
}