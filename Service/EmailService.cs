using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace Service;

public class EmailService(IFluentEmailFactory fluentEmailFactory, ILogger<EmailService> logger)
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var fluentEmail = fluentEmailFactory.Create();
        var response = await fluentEmail
            .To(toEmail)
            .Subject(subject)
            .Body(message)
            .SendAsync();

        if (!response.Successful)
        {
            logger.LogError("An error occurred while sending the email. Errors: {Errors}",
                string.Join(", ", response.ErrorMessages));
        }
    }

    public async Task SendEmailWithTemplateAsync<T>(string toEmail, string subject, string template, T model)
    {
        var fluentEmail = fluentEmailFactory.Create();
        var response = await fluentEmail
            .To(toEmail)
            .Subject(subject)
            .UsingTemplate(template, model)
            .SendAsync();

        if (!response.Successful)
        {
            logger.LogError("An error occurred while sending the email. Errors: {Errors}",
                string.Join(", ", response.ErrorMessages));
        }
    }

    public async Task SendEmailWithTemplateFileAsync<T>(string toEmail, string subject, string templateFileName,
        T model)
    {
        var fluentEmail = fluentEmailFactory.Create();
        var response = await fluentEmail
            .To(toEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templateFileName, model)
            .SendAsync();

        if (!response.Successful)
        {
            logger.LogError("An error occurred while sending the email. Errors: {Errors}",
                string.Join(", ", response.ErrorMessages));
        }
    }
}