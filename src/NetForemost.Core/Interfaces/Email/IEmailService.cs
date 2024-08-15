using Ardalis.Result;

namespace NetForemost.Core.Interfaces.Email;

public interface IEmailService
{
    /// <summary>
    /// Send an email
    /// </summary>
    /// <param name="emailsTo">The emails to</param>
    /// <param name="subject">The email subject</param>
    /// <param name="body">The email body</param>
    /// <param name="isBodyHtml">Validates if the body is HTML or not</param>
    /// <param name="hasAttachment">Valid whether an attachment is sent or not</param>
    /// <param name="attachmentLocation">Local file path</param>
    /// <returns>Mail sent or not</returns>
    Task<Result<bool>> TrySendEmail(string emailsTo, string subject, string body, bool isBodyHtml = false, bool hasAttachment = false, string attachmentLocation = "");
}