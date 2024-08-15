using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class UpdatePasswordAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Update Password
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenUpdatePasswordAsyncIsValid_ReturnSuccess()
    {
        //Delcarations of variables
        var newPassword = "newPassword";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ChangePasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdatePasswordAsync(user.UserName, user.PasswordHash, newPassword);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the password does not comply with the rules and does not finish the process successfully.
    /// </summary>
    /// <returns> Returns that the password does not comply with the rules </returns>
    [Fact]
    public async Task WhenUpdatePasswordNotComplyWithTheRules_ReturnsPasswordRules()
    {
        //Delcarations of variables
        var newPassword = "new";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "old"
        };

        var errors = new IdentityError[]
        {
            new IdentityError() {Code = ",", Description = "Passwords must have" },
            new IdentityError() {Code = ",", Description = "Passwords must be" }
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ChangePasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Failed(errors)));

        var result = await accountService.UpdatePasswordAsync(user.UserName, user.PasswordHash, newPassword);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_PasswordRules);
    }

    /// <summary>
    /// It checks if the current password is not correct and does not finish the process successfully.
    /// </summary>
    /// <returns> Returns Incorrect Password </returns>
    [Fact]
    public async Task WhenPasswordSentIsDiferentToCurrentPassword_ReturnsPasswordIncorrect()
    {
        //Delcarations of variables
        var newPassword = "new";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "old"
        };

        var errors = new IdentityError[]
        {
            new IdentityError() {Code = ",", Description = "Incorrect password" }
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.ChangePasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Failed(errors)));

        var result = await accountService.UpdatePasswordAsync(user.UserName, user.PasswordHash, newPassword);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.IncorrectCurrentPassword);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToUpdatePassword_ReturnsError()
    {
        //Delcarations of variables
        var newPassword = "newPassword";
        var testError = "Error to update Password";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword"
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).Throws(new Exception(testError));

        userManager.Setup(userManager => userManager.ChangePasswordAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            It.IsAny<string>()
            )).Throws(new Exception(testError));

        var result = await accountService.UpdatePasswordAsync(user.UserName, user.PasswordHash, newPassword);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
