using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using System.Security.Claims;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.AuthenticationService;

public class RefreshTokenAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Refresh Token
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenRefreshTokenAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var refreshToken = "Refresh Token";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GetPrincipalFromExpiredToken(
            It.IsAny<string>()
            )).Returns(claimsPrincipal);

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

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.ListAsync(
            It.IsAny<GetUserRefreshTokensByUserIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserRefreshToken>());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(accessToken);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.RefreshTokenAsync(accessToken, refreshToken);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Token sent is'nt correct and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns invalid access token or refresh token </returns>
    [Fact]
    public async Task WhenTokensSentIsInvalid_ReturnsInvalidAccessTokenOrRefreshToken()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var refreshToken = "Refresh Token";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GetPrincipalFromExpiredToken(
            It.IsAny<string>()
            )).Returns(Result<ClaimsPrincipal>.NotFound);

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

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.ListAsync(
            It.IsAny<GetUserRefreshTokensByUserIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserRefreshToken>());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(accessToken);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.RefreshTokenAsync(accessToken, refreshToken);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), "Invalid access token or refresh token");
    }

    /// <summary>
    /// It checks if the Token sent is null and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns invalid access token or refresh token </returns>
    [Fact]
    public async Task WhenTokenValueIsNull_ReturnsInvalidAccessTokenOrRefreshToken()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var refreshToken = "Refresh Token";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GetPrincipalFromExpiredToken(
            It.IsAny<string>()
            )).Returns((ClaimsPrincipal)null);

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

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.ListAsync(
            It.IsAny<GetUserRefreshTokensByUserIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserRefreshToken>());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(accessToken);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.RefreshTokenAsync(accessToken, refreshToken);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), "Invalid access token or refresh token");
    }

    /// <summary>
    /// It checks if the refresh token sent exists and is not available, does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Unauthorized </returns>
    [Fact]
    public async Task WhenRefreshTokenExistAndIsNotAvailable_ReturnsUnauthorized()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var refreshToken = "Refresh Token";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GetPrincipalFromExpiredToken(
            It.IsAny<string>()
            )).Returns(claimsPrincipal);

        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userRefreshTokenRepository.Setup(tokenManagerService => tokenManagerService.FirstOrDefaultAsync(
            null,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.UpdateAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.ListAsync(
            It.IsAny<GetUserRefreshTokensByUserIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserRefreshToken>());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(accessToken);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.RefreshTokenAsync(accessToken, refreshToken);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), "");
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrorsOccurToRefreshToken_ReturnsError()
    {
        // Declarations of variables
        var accessToken = "Access Token";
        var refreshToken = "Refresh Token";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = "test@email.com",
            PasswordHash = "Password Test",
            IsActive = true,
            LockoutEnabled = false
        };

        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out _,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GetPrincipalFromExpiredToken(
            It.IsAny<string>()
            )).Returns(claimsPrincipal);

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

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.ListAsync(
            It.IsAny<GetUserRefreshTokensByUserIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserRefreshToken>());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Throws(new Exception("Error to Refresh Token"));

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(accessToken);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.RefreshTokenAsync(accessToken, refreshToken);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
