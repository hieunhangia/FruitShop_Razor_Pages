using MailKitSimplified.Sender.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Service;

public class EmailService(ILogger<EmailService> logger, IConfiguration configuration)
{
    private readonly string _host = configuration["EmailSender:SmtpHost"]!;
    private readonly ushort _port = ushort.Parse(configuration["EmailSender:SmtpPort"]!);
    private readonly string _username = configuration["EmailSender:Username"]!;
    private readonly string _password = configuration["EmailSender:Password"]!;
    private readonly string _fromName = configuration["EmailSender:FromName"]!;
    private readonly string _fromEmail = configuration["EmailSender:FromEmail"]!;

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            await using var smtpSender = SmtpSender.Create(_host, _port).SetCredential(_username, _password);
            var isSent = await smtpSender.WriteEmail
                .From(_fromName, _fromEmail)
                .To(toEmail)
                .Subject(subject)
                .BodyHtml(message)
                .TrySendAsync();

            if (!isSent)
            {
                logger.LogError("Failed to send email to {ToEmail} with subject {Subject}.", toEmail, subject);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while sending the email.");
        }
    }
}