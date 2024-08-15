using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.AuthenticationService;

public class AuthenticateAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of User Authenticate
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenAuthenticateAsyncIsCorrect_ReturnsSuccess()
    {
        // Declarations of variables
        var email = "test@email.com";
        var password = "Password Test";
        var isEmailConfirmed = true;
        var tokenGenerate = "Token Generate";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = true,
            LockoutEnabled = false
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Success));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check if there is no email that does'nt end the process correctly.
    /// </summary>
    /// <returns> Returns Failed Login </returns>
    [Fact]
    public async Task WhenEmailNotExist_ReturnsFailedLongin()
    {
        // Declarations of variables
        var email = "test@email.com";
        var emailNotExist = "notexist@email.com";
        var password = "Password Test";
        var isEmailConfirmed = true;
        var tokenGenerate = "Token Generate";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = true,
            LockoutEnabled = false
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            emailNotExist
            )).ReturnsAsync(new User());

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Success));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.FailedLogin);
    }

    /// <summary>
    /// Check if the account has already been verified that does'nt terminate the process correctly.
    /// </summary>
    /// <returns> Returns the error that the account has already been verified </returns>
    [Fact]
    public async Task WhenAccountIsNotVerified_ReturnsErrorReconfirmAccount()
    {
        // Declarations of variables
        var email = "test@email.com";
        var password = "Password Test";
        var isEmailConfirmed = false;
        var tokenGenerate = "Token Generate";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = true,
            LockoutEnabled = false
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Success));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_ReConfirmAccount);
    }

    /// <summary>
    /// Check if the password is't correct that does'nt end the process correctly.
    /// </summary>
    /// <returns> Returns Failed Login</returns>
    [Fact]
    public async Task WhenPasswordIsNotCorrectToLogin_ReturnsFailedLogin()
    {
        // Declarations of variables
        var email = "test@email.com";
        var password = "Password Test";
        var isEmailConfirmed = true;
        var tokenGenerate = "Token Generate";
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = true,
            LockoutEnabled = false
        };

        var userInvalid = new User();

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Failed));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.FailedLogin);
    }

    /// <summary>
    /// Check if User is inactive that does'st end the process correctly.
    /// </summary>
    /// <returns> Returns Suspended User </returns>
    [Fact]
    public async Task WhenUserIsInactive_ReturnsSuspendedUser()
    {
        // Declarations of variables
        var email = "test@email.com";
        var password = "Password Test";
        var isEmailConfirmed = true;
        var tokenGenerate = "Token Generate";
        var isActive = false;
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = isActive,
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Success));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Returns(new UserRefreshToken());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SuspendedUser);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrorsOccurToAuthenticate_ReturnsError()
    {
        // Declarations of variables
        var email = "test@email.com";
        var password = "Password Test";
        var isEmailConfirmed = true;
        var tokenGenerate = "Token Generate";
        var isActive = false;
        var jwtConfigValue = new JwtConfig();

        var user = new User()
        {
            Id = "user_id",
            UserName = "username_test",
            Email = email,
            PasswordHash = password,
            IsActive = isActive,
            LockoutEnabled = true
        };

        // Create the simulated service
        var authenticationService = ServiceUtilities.CreateAuthenticacionService(
            out Mock<UserManager<User>> userManager,
            out Mock<SignInManager<User>> signInManager,
            out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
            out _,
            out Mock<ITokenManagerService> tokenManagerService,
            out Mock<IOptions<JwtConfig>> jwtConfig,
            out _
            );

        // Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(isEmailConfirmed);

        signInManager.Setup(signInManager => signInManager.PasswordSignInAsync(
            It.IsAny<User>(),
            It.IsAny<string>(),
            false,
            false
            )).Returns(Task.FromResult(SignInResult.Success));

        userManager.Setup(userManager => userManager.GetLockoutEndDateAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new DateTimeOffset());

        userManager.Setup(userManager => userManager.GetRolesAsync(
            It.IsAny<User>()
            )).ReturnsAsync(new List<string>());

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateRefreshToken(
            It.IsAny<User>()
            )).Throws(new Exception("An Errors Occur to Authenticate"));

        tokenManagerService.Setup(tokenManagerService => tokenManagerService.GenerateAccessToken(
            It.IsAny<User>(),
            It.IsAny<List<string>>()
            )).Returns(tokenGenerate);

        userRefreshTokenRepository.Setup(userRefreshTokenRepository => userRefreshTokenRepository.AddAsync(
            It.IsAny<UserRefreshToken>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new UserRefreshToken());

        jwtConfig.Setup(jwtConfig => jwtConfig.Value).Returns(jwtConfigValue);

        var result = await authenticationService.AuthenticateAsync(email, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
