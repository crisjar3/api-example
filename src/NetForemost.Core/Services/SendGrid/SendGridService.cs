using Ardalis.Result;
using Microsoft.Extensions.Options;
using NetForemost.Core.Entities.SendGrid;
using NetForemost.Core.Interfaces.SendGrid;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetForemost.Core.Services.SendGrid;

public class SendGridService : ISendGridService
{
    private SendGridApiConfig _sendGridApiConfig;

    public SendGridService(IOptions<SendGridApiConfig> sendGridConfig)
    {
        _sendGridApiConfig = sendGridConfig.Value;
    }

    public async Task<Result<bool>> TrySendEmailAsync(string subject, string htmlContent, string textContent, string emailsTo, string emailToName, string? AddAttachmentLocation = null, string? fileName = null)
    {
        var apiKey = _sendGridApiConfig.ApiKey;
        var client = new SendGridClient(apiKey);

        var sendGridMessage = new SendGridMessage()
        {
            From = new EmailAddress(_sendGridApiConfig.Email, _sendGridApiConfig.EmailFrom),
            Subject = subject,
            PlainTextContent = textContent,
            HtmlContent = htmlContent
        };

        //Check if the email has any attachments
        if (AddAttachmentLocation is not null && fileName is not null)
        {
            using var fileStream = File.OpenRead(AddAttachmentLocation);
            await sendGridMessage.AddAttachmentAsync(fileName, fileStream);
        }

        //Check if there is more than one address to add
        var emails = emailsTo.Split(";");

        foreach (string emailAddress in emails)
        {
            sendGridMessage.AddTo(emailAddress);
        }


        //Sending the email
        var response = await client.SendEmailAsync(sendGridMessage);

        if (response.IsSuccessStatusCode)
        {
            return Result<bool>.Success(true);
        }
        else
        {
            return Result<bool>.Error(response.Headers+ " " + response.Body);
        }
    }
}
