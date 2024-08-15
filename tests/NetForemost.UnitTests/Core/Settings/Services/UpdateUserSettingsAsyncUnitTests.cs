using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Languages;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Settings.Services;

public class UpdateUserSettingsAsyncUnitTests
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
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };
        var languageRepository = new Mock<IAsyncRepository<Language>>();
        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository, out _, languageRepository);

        // Configurations for tests
        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        languageRepository.Setup(languageRepository => languageRepository.FirstOrDefaultAsync(
            It.IsAny<FindLanguageByIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Language());

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.UpdateAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await userSettingsService.UpdateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the UserSettings does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return UserSettings NotExist </returns>
    [Fact]
    public async Task WhenUserSettingsNotExist_ReturnUserSettingsNotExist()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };

        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository, out _);

        // Configurations for tests
        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((UserSettings)null);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.UpdateAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await userSettingsService.UpdateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserSettingsNotExist);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToUpdateUserSettings_ReturnError()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };
        var testError = "Error to Update a UserSettings";

        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository, out _);

        // Configurations for tests
        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(userSettings);

        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.UpdateAsync(
            It.IsAny<UserSettings>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await userSettingsService.UpdateUserSettingsAsync(userSettings);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
