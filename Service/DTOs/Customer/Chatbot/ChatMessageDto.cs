using Microsoft.SemanticKernel.ChatCompletion;

namespace Service.DTOs.Customer.Chatbot;

public class ChatMessageDto
{
    public required AuthorRole Role { get; set; }
    public required string Content { get; set; }
}