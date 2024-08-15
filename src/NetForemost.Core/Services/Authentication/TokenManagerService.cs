using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Enumerations.Language;
using NetForemost.Core.Interfaces.Authentication;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetForemost.Core.Services.Authentication;

public class TokenManagerService : ITokenManagerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfig _jwtConfig;
    private readonly IDatabase _redisDb;
    private readonly IConfiguration _configuration;

    public TokenManagerService(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfig> jwtConfig, IConnectionMultiplexer redisConection,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtConfig = jwtConfig.Value;
        _redisDb = redisConection.GetDatabase();
        _configuration = configuration;
    }

    #region REDIS METHODS

    public async Task<bool> IsCurrentActiveToken()
    {
        return await IsActiveAsync(GetCurrentAsync());
    }

    public async Task DeactivateCurrentAsync()
    {
        await DeactivateAsync(GetCurrentAsync());
    }

    public async Task<bool> IsActiveAsync(string token)
    {
        return string.IsNullOrEmpty((await _redisDb.StringGetAsync(GetKey(token))).ToString());
    }

    public async Task DeactivateAsync(string token)
    {
        await _redisDb.StringSetAsync(GetKey(token), " ", TimeSpan.FromMinutes(_jwtConfig.AuthTokenValidityInMins));
    }

    private string GetCurrentAsync()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];

        return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
    }

    private static string GetKey(string token)
    {
        return $"TOKEN:{token}:Desactivated";
    }

    #endregion

    public Result<string> GenerateAccessToken(User user, List<string> roles)
    {
        try
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, $"{user.UserName}"),
                new(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new(ClaimTypes.Email, $"{user.Email}"),
                new(CustomClaimType.Language, $"{user.UserSettings.LanguageId}"),
                new(CustomClaimType.LanguageCode, $"{user.UserSettings.Language.Code}")
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, $"{role}")));

            // Payload
            var payload = new JwtPayload(_configuration["Authentication:Jwt:Issuer"], _configuration["Authentication:Jwt:Audience"], claims,
                DateTime.Now, DateTime.UtcNow.AddMinutes(_jwtConfig.AuthTokenValidityInMins));

            var securityToken = new JwtSecurityToken(header, payload);
            var securityTokenValue = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return Result<string>.Success(securityTokenValue);
        }
        catch (Exception ex)
        {
            return Result<string>.Error(ex.Message);
        }
    }

    public Result<UserRefreshToken> GenerateRefreshToken(User user)
    {
        try
        {
            var userRefreshToken = new UserRefreshToken
            {
                Active = true,
                Expiration = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenValidityInDays),
                Value = Guid.NewGuid().ToString("N"),
                Used = false,
                UserId = user.Id,
                CreatedBy = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            return Result<UserRefreshToken>.Success(userRefreshToken);
        }
        catch (Exception ex)
        {
            return Result<UserRefreshToken>.Error(ex.Message);
        }
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Result<ClaimsPrincipal>.Success(principal);
        }
        catch (Exception)
        {
            return Result<ClaimsPrincipal>.Error("Invalid Token");
        }
    }
}