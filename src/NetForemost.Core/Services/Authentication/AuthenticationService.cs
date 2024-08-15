using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetForemost.Core.Dtos.Authorizations;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Account;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Specifications.AppClients;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAsyncRepository<UserRefreshToken> _userRefreshTokenRepository;
    private readonly IAsyncRepository<AppClient> _appClientRepository;
    private readonly IAccountService _accountService;
    private readonly ITokenManagerService _tokenManagerService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IAsyncRepository<UserRefreshToken> userRefreshTokenRepository,
        IAccountService accountService, ITokenManagerService tokenManagerService, IOptions<JwtConfig> jwtConfig, IAsyncRepository<AppClient> appClientRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRefreshTokenRepository = userRefreshTokenRepository;
        _accountService = accountService;
        _tokenManagerService = tokenManagerService;
        _jwtConfig = jwtConfig.Value;
        _appClientRepository = appClientRepository;
    }

    public async Task<Result<AuthorizedUserDto>> AuthenticateAsync(string email, string password)
    {
        //Verify that the email exists in the database
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return Result<AuthorizedUserDto>.Invalid(new List<ValidationError> { new ValidationError { ErrorMessage = ErrorStrings.FailedLogin } });

        //Verify that the account has already been verified
        var isVerified = await _userManager.IsEmailConfirmedAsync(user);

        if (!isVerified)
        {
            return Result<AuthorizedUserDto>.Invalid(new List<ValidationError> { new ValidationError { ErrorMessage = ErrorStrings.Result_ReConfirmAccount } });
        }

        //Verify that the password is correct to log in
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (!result.Succeeded) return Result<AuthorizedUserDto>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.FailedLogin } });

        //Check if the user has an active account lockout
        if (result.IsLockedOut)
        {
            //Performs the process of converting the lockout time to the user's time zone
            DateTimeOffset lockoutEnd = (DateTimeOffset)await _userManager.GetLockoutEndDateAsync(user);
            return Result<AuthorizedUserDto>.Invalid(new() { new() { ErrorMessage = ErrorStrings.LockedAccount.Replace("[LockedTimeEnd]", lockoutEnd.UtcDateTime.AddHours(user.TimeZone.Offset).ToString("hh:mm tt")) } });
        }

        //Check if the user's account is suspended
        if (!user.IsActive) return Result<AuthorizedUserDto>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.SuspendedUser } });

        var userRoles = await _userManager.GetRolesAsync(user);
        var newRefreshToken = _tokenManagerService.GenerateRefreshToken(user).Value;
        var newAccessToken = _tokenManagerService.GenerateAccessToken(user, userRoles.ToList()).Value;

        await _userRefreshTokenRepository.AddAsync(newRefreshToken);

        var authorizedUser = new AuthorizedUserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            AccessToken = newAccessToken,
            AuthTokenValidityInMins = _jwtConfig.AuthTokenValidityInMins,
            RefreshToken = newRefreshToken.Value,
            RefreshTokenValidityInDays = _jwtConfig.RefreshTokenValidityInDays,
            UserImageUrl = user.UserImageUrl
        };

        return Result<AuthorizedUserDto>.Success(authorizedUser);
    }

    public async Task<Result<AuthorizedUserDto>> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        try
        {
            //Verify that the tokens sent are valid tokens
            var resultClaims = _tokenManagerService.GetPrincipalFromExpiredToken(accessToken);

            if (!resultClaims.IsSuccess) return Result<AuthorizedUserDto>.Invalid(new() { new() { ErrorMessage = "Invalid access token or refresh token" } });

            if (resultClaims.Value is null) return Result<AuthorizedUserDto>.Invalid(new() { new() { ErrorMessage = "Invalid access token or refresh token" } });

            var user = await _userManager.FindByNameAsync(resultClaims.Value.Identity.Name);

            //Verifies that the refresh token sent exists and is available
            var userRefreshToken = await _userRefreshTokenRepository.FirstOrDefaultAsync(new GetUserRefreshTokenByRefreshTokenSpecification(user.Id, refreshToken));

            if (userRefreshToken == null) return Result<AuthorizedUserDto>.Unauthorized();

            //Refreshes and expires the availability of the sent token
            userRefreshToken.Active = false;
            userRefreshToken.Used = true;
            userRefreshToken.UpdatedBy = user.Id;
            userRefreshToken.UpdatedAt = DateTime.UtcNow;

            await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);

            //Checks if expired tokens are found in the DB and updates them
            var userRefreshTokens = await _userRefreshTokenRepository.ListAsync(new GetUserRefreshTokensByUserIdSpecification(user.Id));

            foreach (var refreshTokenDB in userRefreshTokens)
            {
                if (refreshTokenDB.Expiration <= DateTime.UtcNow)
                {
                    refreshTokenDB.Active = false;
                    refreshTokenDB.UpdatedBy = user.Id;
                    refreshTokenDB.UpdatedAt = DateTime.UtcNow;

                    await _userRefreshTokenRepository.UpdateAsync(refreshTokenDB);
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var newRefreshToken = _tokenManagerService.GenerateRefreshToken(user).Value;
            var newAccessToken = _tokenManagerService.GenerateAccessToken(user, userRoles.ToList()).Value;

            await _userRefreshTokenRepository.AddAsync(newRefreshToken);

            //Generate new tokens and access
            var authorizedUser = new AuthorizedUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                AccessToken = newAccessToken,
                AuthTokenValidityInMins = _jwtConfig.AuthTokenValidityInMins,
                RefreshToken = newRefreshToken.Value,
                RefreshTokenValidityInDays = _jwtConfig.RefreshTokenValidityInDays
            };

            return Result<AuthorizedUserDto>.Success(authorizedUser);
        }
        catch (Exception ex)
        {
            return Result<AuthorizedUserDto>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken, string userName)
    {
        try
        {
            //Disable Access token
            await _tokenManagerService.DeactivateCurrentAsync();

            //Disable refresh token
            var user = await _userManager.FindByNameAsync(userName);

            var userRefreshToken = await _userRefreshTokenRepository.FirstOrDefaultAsync(new GetUserRefreshTokenByRefreshTokenSpecification(user.Id, refreshToken));

            if (userRefreshToken is not null)
            {
                userRefreshToken.Active = false;
                userRefreshToken.Used = false;
                userRefreshToken.UpdatedBy = user.Id;
                userRefreshToken.UpdatedAt = DateTime.UtcNow;

                await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<bool>> VerifyApiKey(string apiKey)
    {
        try
        {
            //Validate is correct format the api key
            var guidApiKey = new Guid();

            var validApiKey = Guid.TryParse(apiKey, out guidApiKey);

            if (!validApiKey)
            {
                return Result<bool>.Invalid(new()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.ApikeyInvalidFormat.Replace("[apiKey]", apiKey)
                    }
                });
            }

            //verify exist apiKey
            var existApiKey = await _appClientRepository.AnyAsync(new GetAppClientByApiKeySpecification(guidApiKey));

            if (existApiKey)
                return Result<bool>.Success(true);
            return Result<bool>.Invalid(new()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.ApiKeyNotFound.Replace("[apiKey]", apiKey)
                }
            });
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<AppClient>> CreateAppClientAsync(AppClient appClient, string userId)
    {
        try
        {
            //set default values
            appClient.CreatedBy = userId;
            appClient.CreatedAt = DateTime.UtcNow;

            //set the new apiKey
            appClient.ApiKey = Guid.NewGuid();

            await _appClientRepository.AddAsync(appClient);

            return Result<AppClient>.Success(appClient);
        }
        catch (Exception ex)
        {
            return Result<AppClient>.Error(ex.Message + ex.InnerException);
        }

    }
}