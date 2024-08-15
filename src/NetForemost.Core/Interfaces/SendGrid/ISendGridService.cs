using Ardalis.Result;

namespace NetForemost.Core.Interfaces.SendGrid;

public interface ISendGridService
{
    /// <summary>
    /// Try sending an email through the official SendGrid dependency
    /// </summary>
    /// <param name="subject">Email Subject</param>
    /// <param name="htmlContent">HTML content when the email makes use of this</param>
    /// <param name="textContent">Plain text content when the email makes use of this</param>
    /// <param name="emailTo">Email recipient's email</param>
    /// <param name="emailToName">Name of the recipient of the email</param>
    /// <returns>Returns whether or not the process is finished</returns>
    Task<Result<bool>> TrySendEmailAsync(string subject, string htmlContent, string textContent, string emailTo, string emailToName, string? AddAttachmentLocation = null, string? fileName = null);
}
