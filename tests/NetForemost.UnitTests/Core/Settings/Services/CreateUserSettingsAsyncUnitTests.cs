using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Languages;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Settings.Services;

public class CreateUserSettingsAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to create an UserSettings
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateUserSettingsIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id, Language = new Language() { Id = 1 }, LanguageId = 1 };
        var languageRepository = new Mock<IAsyncRepository<Language>>();
        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(
            out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository,
            out Mock<UserManager<User>> userManager, languageRepository);

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetBySpecAsync(
            It.IsAny<GetUserSettingsByIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((UserSettings)null);

        languageRepository.Setup(languageRepository => languageRepository.FirstOrDefaultAsync(
           It.IsAny<FindLanguageByIdSpecification>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(new Language());

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.AddAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        var result = await userSettingsService.CreateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the User does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return User NotExist </returns>
    [Fact]
    public async Task WhenUserNotExist_ReturnUserNotExist()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };

        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(
            out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository,
            out Mock<UserManager<User>> userManager);

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync((User)null);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetBySpecAsync(
            It.IsAny<GetUserSettingsByIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((UserSettings)null);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.AddAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        var result = await userSettingsService.CreateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserNotExist);
    }

    /// <summary>
    /// It checks if the UserSettings exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return User SettingsExisting </returns>
    [Fact]
    public async Task WhenUserSettingsExisting_ReturnUserSettingsExisting()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };
        var languageRepository = new Mock<IAsyncRepository<Language>>();
        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(
            out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository,
            out Mock<UserManager<User>> userManager, languageRepository);

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetBySpecAsync(
            It.IsAny<GetUserSettingsByIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        languageRepository.Setup(languageRepository => languageRepository.FirstOrDefaultAsync(
           It.IsAny<FindLanguageByIdSpecification>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(new Language());

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.AddAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        var result = await userSettingsService.CreateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserSettingsExisting);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToCreateUserSettings_ReturnError()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };
        var testError = "Error to Create a new UserSettings";

        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(
            out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository,
            out Mock<UserManager<User>> userManager);

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetBySpecAsync(
            It.IsAny<GetUserSettingsByIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((UserSettings)null);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.AddAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await userSettingsService.CreateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
