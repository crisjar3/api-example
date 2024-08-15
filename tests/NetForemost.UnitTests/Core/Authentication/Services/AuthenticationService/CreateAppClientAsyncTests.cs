using Moq;
using NetForemost.Core.Entities.AppClients;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.AuthenticationService;

public class CreateAppClientAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create App Client
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateAppClientIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var appClient = new AppClient();
        var userId = "user_id";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AddAsync(
            It.IsAny<AppClient>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(appClient);

        var result = await authenticationService.CreateAppClientAsync(appClient, userId);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccur_ReturnsError()
    {
        // Declarations of variables
        var appClient = new AppClient();
        var userId = "user_id";

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out _, out _, out _, out _, out _, out _,
            out Mock<IAsyncRepository<AppClient>> appClientRepository
            );

        // Configurations for tests
        appClientRepository.Setup(appClientRepository => appClientRepository.AddAsync(
            It.IsAny<AppClient>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception("An Unexpected Error Occur to Create AppClient"));

        var result = await authenticationService.CreateAppClientAsync(appClient, userId);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}

