using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Email;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class CreateHireUserUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create Hire User
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateUserIsValid_ReturnSuccess()
    {
        // Declaration of variables
        var password = "1234";
        var tokenTest = "this is a token test";

        var user = new User()
        {
            UserName = "user_test",
            FirstName = "New User FN",
            LastName = "New User LN",
            Email = "test@email.com"
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(), It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.CreateHireUser(user, password);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if username exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Username not available </returns>
    [Fact]
    public async Task WhenUsernameExist_ReturnsUsernameNotAvailable()
    {
        // Declaration of variables
        var password = "1234";
        User user = new User()
        {
            Id = "user_id",
            UserName = "username test",
            FirstName = "FN Test",
            LastName = "LN Test",

        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await accountService.CreateHireUser(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// It checks if email exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email has been already registered </returns>
    [Fact]
    public async Task WhenEmaiUserlSentExist_ReturnsEmailHasbeenAlreadyRegistered()
    {
        // Declaration of variables
        var username = "user_email_exist";
        var password = "1234";
        var email = "test@email.com";
        User user = new User() { UserName = username, Email = email };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await accountService.CreateHireUser(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// It checks if city not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns city not found </returns>
    [Fact]
    public async Task WhenCityNotExist_ReturnsCityNotFound()
    {
        // Declaration of variables
        var password = "1234";
        var tokenTest = "this is a token test";
        var cityIdNotExist = 10;

        var user = new User()
        {
            UserName = "user_test",
            FirstName = "New User FN",
            LastName = "New User LN",
            Email = "test@email.com",
            CityId = 1
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(), It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            cityIdNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.CreateHireUser(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CityNotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnsError()
    {
        // Declaration of variables
        var password = "1234";
        var tokenTest = "this is a token test";
        var testError = "Error to create userr";

        var user = new User()
        {
            UserName = "user_test",
            FirstName = "New User FN",
            LastName = "New User LN",
            Email = "test@email.com"
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(), It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).Throws(new Exception(testError));

        var result = await accountService.CreateHireUser(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
