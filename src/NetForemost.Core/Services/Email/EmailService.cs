using Ardalis.Result;
using Microsoft.Extensions.Options;
using NetForemost.Core.Entities.Emails;
using NetForemost.Core.Interfaces.Email;
using System.Net.Mail;
using System.Text;

namespace NetForemost.Core.Services.Email;

public class EmailService : IEmailService
{
    private readonly ISmtpClient _smtp;
    private readonly SmtpClientConfig _smtpClientConfig;

    public EmailService(IOptions<SmtpClientConfig> options, ISmtpClient smtp)
    {
        _smtp = smtp;
        _smtpClientConfig = options.Value;
    }

    public async Task<Result<bool>> TrySendEmail(string emailsTo, string subject, string body, bool isBodyHtml = false, bool hasAttachment = false, string attachmentLocation = "")
    {
        try
        {
            MailMessage email = new()
            {
                From = new MailAddress(_smtpClientConfig.UserName, "NetForemost", Encoding.UTF8)
            };

            var emails = emailsTo.Split(";");

            foreach (var emailAddress in emails) email.To.Add(emailAddress);

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = isBodyHtml;
            email.Priority = MailPriority.Normal;

            if (hasAttachment)
                using (var attachment = new Attachment(attachmentLocation))
                {
                    email.Attachments.Add(attachment);
                    await _smtp.SendMailAsync(email);
                }
            else
                await _smtp.SendMailAsync(email);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}