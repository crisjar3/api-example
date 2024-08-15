using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class GetUserByIdAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Get user by Id
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public Task WhenGetUserByIDIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var isEmailConfirmed = false;
        var user = new User()
        {
            Id = "user test",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        var users = new List<User>() { user };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        var mock = users.AsQueryable().BuildMock();

        //Configurations for tests
        userManager.Setup(userManager => userManager.Users).Returns(mock);

        var result = accountService.GetUserByIdAsync(user.Id);

        // Validations for tests
        Assert.True(result.Result.IsSuccess);
        return Task.CompletedTask;
    }

    /// <summary>
    /// It checks if the user not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns User Not Found </returns>
    [Fact]
    public async Task WhenUserIdNotExist_UserNotFound()
    {
        //Delcarations of variables
        var isEmailConfirmed = false;
        var idNotRegistered = "Id not Registered";
        var user = new User()
        {
            Id = "Id registered",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        var users = new List<User>() { user };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        var mock = users.AsQueryable().BuildMock();

        //Configurations for tests
        userManager.Setup(userManager => userManager.Users).Returns(mock);

        var result = await accountService.GetUserByIdAsync(idNotRegistered);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetErrors(result.Errors.ToList()), ErrorStrings.User_NotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public Task WhenUnexpectedErroOccur_ReturnsError()
    {
        //Delcarations of variables
        var isEmailConfirmed = false;
        var user = new User()
        {
            Id = "user test",
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = isEmailConfirmed
        };

        var users = new List<User>()
        {
            new()
            {
                Id = "user test",
                UserName = "username",
                PasswordHash = "oldPassword",
                Email = "test@email.com",
                EmailConfirmed = isEmailConfirmed
            }

        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        var mock = users.AsQueryable().BuildMock();

        //Configurations for tests
        userManager.Setup(userManager => userManager.Users).Throws(new Exception());

        var result = accountService.GetUserByIdAsync(user.Id);

        // Validations for tests
        Assert.False(result.Result.IsSuccess);
        return Task.CompletedTask;
    }
}
