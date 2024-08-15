using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.AuthenticationService;

public class LogoutAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Logout
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenLogoutIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var username = "user_test";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = username,
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out _,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.DeactivateCurrentAsync()).Returns(Task.CompletedTask);

        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userRefreshTokenRepository.Setup(tokenManagerService => tokenManagerService.FirstOrDefaultAsync(
            It.IsAny<GetUserRefreshTokenByRefreshTokenSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.UpdateAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await authenticationService.LogoutAsync(accessToken, username);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrorsOccurToLogoutAsync_ReturnsError()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var username = "user_test";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = username,
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out _,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.DeactivateCurrentAsync()).Returns(Task.CompletedTask);

        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userRefreshTokenRepository.Setup(tokenManagerService => tokenManagerService.FirstOrDefaultAsync(
            It.IsAny<GetUserRefreshTokenByRefreshTokenSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.UpdateAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception("Error Logout"));

        var result = await authenticationService.LogoutAsync(accessToken, username);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}

