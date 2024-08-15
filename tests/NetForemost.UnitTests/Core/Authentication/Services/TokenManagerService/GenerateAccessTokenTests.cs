using Microsoft.Extensions.Configuration;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.TokenManagerService;

public class GenerateAccessTokenTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Generate Access Token
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenGenerateAccessTokenIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var user = new User()
        {
            Id = "user_id",
            UserName = "Username Test",
            Email = "test@email.com",
            UserSettings = new()
            {
                Language = new()
            }
        };
        var roles = new List<string>();
        var key = "03RCxp/GT/48EeliJG9L/ZS/ITwGAYUUoALJSePkG5k";

        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(mockSection => mockSection.Value).Returns(key);

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _, out _, out _,
            out Mock<IConfiguration> configuration
            );

        // Configurations for tests
        configuration.Setup(configuration => configuration["Authentication:Jwt:SecretKey"]).Returns(key);

        var result = tokenManagerService.GenerateAccessToken(user, roles);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToGenerateAccessToken_ReturnsError()
    {
        // Declarations of variables
        var user = new User()
        {
            Id = "user_id",
            UserName = "Username Test",
            Email = "test@email.com"
        };
        var roles = new List<string>();
        var key = "03RCxp/GT/48EeliJG9L/ZS/ITwGAYUUoALJSePkG5k";

        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(mockSection => mockSection.Value).Returns(key);

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _, out _, out _,
            out Mock<IConfiguration> configuration
            );

        // Configurations for tests
        configuration.Setup(configuration => configuration["Authentication:Jwt:SecretKey"]
        ).Throws(new Exception("An Unexpected Erros Occur to Generate Access Token"));

        var result = tokenManagerService.GenerateAccessToken(user, roles);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
