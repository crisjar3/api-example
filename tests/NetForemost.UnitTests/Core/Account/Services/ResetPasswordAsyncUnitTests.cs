using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class ResetPasswordAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Reset Password
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenGenerateResetPasswordIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var encodedToken = "Is a  encodedToken";
        var password = "password";
        var user = new User()
        {
            Id = "UserID",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ResetPasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.ResetPasswordAsync(user.Id, Base64Helper.Encode(encodedToken), password);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the User not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns User Not Found </returns>
    [Fact]
    public async Task WhenUserIdNotExist_ReturnsUserNotFound()
    {
        //Delcarations of variables
        var encodedToken = "Is a  encodedToken";
        var idNotRegistered = "Id Not registed";
        var password = "password";
        var user = new User()
        {
            Id = "UserID",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            idNotRegistered
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ResetPasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.ResetPasswordAsync(user.Id, Base64Helper.Encode(encodedToken), password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ResetPasswordTittle)} {ErrorStrings.Result_ReOpenFromEmail_Error}");
    }

    /// <summary>
    /// It checks if the token is decoded and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns token not encoded base64 </returns>
    [Fact]
    public async Task WhenTokenIsNotEncoded_ReturnsError()
    {
        //Delcarations of variables
        var encodedToken = "Is a  encodedToken";
        var password = "password";
        var user = new User()
        {
            Id = "UserID",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ResetPasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.ResetPasswordAsync(user.Id, encodedToken, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ResetPasswordTittle)} {ErrorStrings.Result_ReOpenFromEmail_Error}");
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToResetPassword_ReturnsError()
    {
        //Delcarations of variables
        var encodedToken = "Is a  encodedToken";
        var password = "password";
        var user = new User()
        {
            Id = "UserID",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ResetPasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Throws(new Exception());

        var result = await accountService.ResetPasswordAsync(user.Id, encodedToken, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}

