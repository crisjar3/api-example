using Moq;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Specifications.AppClients;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.AuthenticationService;

public class VerifyApiKeyTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Verify Api Key
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenVerifyApiKeyIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var apiKey = "CA761232-ED42-11CE-BACD-00AA0057B223";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AnyAsync(
            It.IsAny<GetAppClientByApiKeySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        var result = await authenticationService.VerifyApiKey(apiKey);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Api key format is not correct and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Api key invalid format </returns>
    [Fact]
    public async Task WhenApiKeyFormatIsInvalid_ReturnsApiKeyInvalidFormat()
    {
        // Declarations of variables
        var apiKey = "asdf123432safd68iuy3asdf123432sa";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AnyAsync(
            It.IsAny<GetAppClientByApiKeySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        var result = await authenticationService.VerifyApiKey(apiKey);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.ApikeyInvalidFormat.Replace("[apiKey]", apiKey)}");
    }

    /// <summary>
    /// It checks if that Api key does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Api Key not found </returns>
    [Fact]
    public async Task WhenApiKeyNotExist_ReturnsApiKeyNotFound()
    {
        // Declarations of variables
        var apiKey = "CA761232-ED42-11CE-BACD-00AA0057B223";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AnyAsync(
            It.IsAny<GetAppClientByApiKeySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        var result = await authenticationService.VerifyApiKey(apiKey);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), $"{ErrorStrings.ApiKeyNotFound.Replace("[apiKey]", apiKey)}");
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrorsOccur_ReturnsError()
    {
        // Declarations of variables
        var apiKey = "CA761232-ED42-11CE-BACD-00AA0057B223";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AnyAsync(
            It.IsAny<GetAppClientByApiKeySpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception("An unexpected error to verify Api Key"));

        var result = await authenticationService.VerifyApiKey(apiKey);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}

