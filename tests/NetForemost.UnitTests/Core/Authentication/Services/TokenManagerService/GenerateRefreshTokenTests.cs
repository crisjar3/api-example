using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.TokenManagerService;

public class GenerateRefreshTokenTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Generate Refresh Token
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
            Email = "test@email.com"
        };

        var jwtConfigValue = new JwtConfig();

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _, out _
            );

        // Configurations for tests
        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = tokenManagerService.GenerateRefreshToken(user);

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
        var user = new User()
        {
            Id = "user_id",
            UserName = "Username Test",
            Email = "test@email.com"
        };

        var jwtConfigValue = new JwtConfig();

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _, out _
            );

        // Configurations for tests
        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = tokenManagerService.GenerateRefreshToken((User)null);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
