using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.Emails;
using NetForemost.Core.Services.Email;
using NetForemost.SharedKernel.Helpers;
using NetForemost.UnitTests.Common;
using System.Net.Mail;
using Xunit;

namespace NetForemost.UnitTests.Core.Email.Services;

public class TrySendEmailTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Send a mail when all is correct.
    /// </summary>
    /// <returns> Return success </returns>
    [Fact]
    public async Task WhenEmailSendIsCorrect_ReturnSuccess()
    {
        // Declaration of variables
        var emailsTo = "fmdina.test@gmail.com";
        var subject = "Test";
        var body = "Is a test";
        var isBodyHtml = true;
        var hasAttachment = false;
        var attachmentLocation = "";

        // Create the simulate service
        var emailService = ServiceUtilities.CreateEmailService(
            out Mock<IOptions<SmtpClientConfig>> options,
            out _
            );

        // Configuration for tests
        options.Setup(options => options.Value).Returns(new SmtpClientConfig());

        var result = await emailService.TrySendEmail(
            emailsTo,
            subject,
            body,
            isBodyHtml,
            hasAttachment,
            attachmentLocation);

        // Validation for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
	/// Verify that if an unexpected error occurs it is caught and does not break the process.
	/// </summary>
	/// <returns>Return error</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnError()
    {
        // Declaration of variables
        var emailsTo = "fmdina.test@gmail.com";
        var subject = "Test";
        var body = "Is a test";
        var isBodyHtml = true;
        var hasAttachment = false;
        var attachmentLocation = "";
        var testError = "Error to try send an Email.";

        // Create the simulate service
        var emailService = ServiceUtilities.CreateEmailService(
            out Mock<IOptions<SmtpClientConfig>> options,
            out Mock<ISmtpClient> smtp
            );

        // Configuration for tests
        options.Setup(options => options.Value).Returns(new SmtpClientConfig());

        smtp.Setup(smtp => smtp.SendMailAsync(
            It.IsAny<MailMessage>()
            )).Throws(new Exception(testError));

        var result = await emailService.TrySendEmail(
            emailsTo,
            subject,
            body,
            isBodyHtml,
            hasAttachment,
            attachmentLocation);

        // Validation for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
