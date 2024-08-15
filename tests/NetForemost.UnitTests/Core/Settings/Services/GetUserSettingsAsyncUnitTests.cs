using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Settings.Services;

public class GetUserSettingsAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to get an UserSettings
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenGetUserSettingsIsCorrect_ReturnSuccess()
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
            )).ReturnsAsync(userSettings);

        var result = await userSettingsService.GetUserSettingsAsync(userSettings.Id);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the UserSettings does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return UserSettings NotExist </returns>
    [Fact]
    public async Task WhenUserSettingsNotExist_ReturnUserNotExist()
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

        var result = await userSettingsService.GetUserSettingsAsync(userSettings.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserSettingsNotExist);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToGetAnUserSettings_ReturnError()
    {
        // Declarations of variables
        var user = new User() { Id = "UserID" };
        var userSettings = new UserSettings() { Id = 1, UserId = user.Id };
        var testError = "Error to Get an UserSettings";

        // Create the simulated service
        var userSettingsService = ServiceUtilities.CreateUserSettingsService(out _, out Mock<IAsyncRepository<UserSettings>> userSettingsRepository, out _);

        // Configurations for tests
        userSettingsRepository.Setup(userSettingsRepository => userSettingsRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await userSettingsService.GetUserSettingsAsync(userSettings.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
