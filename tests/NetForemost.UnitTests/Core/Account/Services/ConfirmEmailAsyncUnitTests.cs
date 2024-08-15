using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Email;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class ConfirmEmailAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Confirm Email
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenConfirmEmailIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        string encodedToken = "Is a test Token";
        var isEmailConfirmed = false;
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(user.EmailConfirmed);

        userManager.Setup(userManager => userManager.ConfirmEmailAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.ConfirmEmailAsync(user.Id, Base64Helper.Encode(encodedToken));

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the email is not correct and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email Not Found </returns>
    [Fact]
    public async Task WhenEmailIsNotRegistered_ReturnsEmailNotFound()
    {
        //Delcarations of variables
        string encodedToken = "Is a test Token";
        var isEmailConfirmed = false;
        string idRegistered = "Id Not Registered";
        var user = new User()
        {
            Id = "valid",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            idRegistered
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(user.EmailConfirmed);

        userManager.Setup(userManager => userManager.ConfirmEmailAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.ConfirmEmailAsync(user.Id, Base64Helper.Encode(encodedToken));

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ConfirmAccountTittle)}, {ErrorStrings.Result_ReOpenFromEmail_Error}");
    }

    /// <summary>
    /// It checks if the email is already confirmed and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns the email has already been confirmed </returns>
    [Fact]
    public async Task WhenEmailIsConfirmed_ReturnsEmailHasAlreadyBeenConfirmed()
    {
        //Delcarations of variables
        string encodedToken = "Is a test Token";
        var isEmailConfirmed = true;
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(user.EmailConfirmed);

        userManager.Setup(userManager => userManager.ConfirmEmailAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.ConfirmEmailAsync(user.Id, Base64Helper.Encode(encodedToken));

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_GenericCompletedSecurityProcess_Error);
    }

    /// <summary>
    /// It checks if the token code comes in base64 and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns a Generic Security Error </returns>
    [Fact]
    public async Task WhenTokenIsDecoded_ReturnsGenericSecurityError()
    {
        //Delcarations of variables
        string encodedToken = "Is a test Token";
        var isEmailConfirmed = false;
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(user.EmailConfirmed);

        userManager.Setup(userManager => userManager.ConfirmEmailAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.ConfirmEmailAsync(user.Id, Base64Helper.Decode(encodedToken));

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ConfirmAccountTittle)}, {ErrorStrings.Result_ReOpenFromEmail_Error}");
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToConfirmEmail_ReturnsError()
    {
        //Delcarations of variables
        string encodedToken = "Is a test Token";
        var isEmailConfirmed = false;
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(user.EmailConfirmed);

        userManager.Setup(userManager => userManager.ConfirmEmailAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Throws(new Exception());

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.ConfirmEmailAsync(user.Id, Base64Helper.Encode(encodedToken));

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
