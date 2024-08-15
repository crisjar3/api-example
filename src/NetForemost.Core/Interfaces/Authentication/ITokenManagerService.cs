using Ardalis.Result;
using NetForemost.Core.Entities.Users;
using System.Security.Claims;

namespace NetForemost.Core.Interfaces.Authentication;

public interface ITokenManagerService
{
    /// <summary>
    ///     Check if the user's current token is active.
    /// </summary>
    /// <returns>Returns true if it is active or false if it is not active.</returns>
    Task<bool> IsCurrentActiveToken();

    /// <summary>
    ///     Deactivate the user's current token.
    /// </summary>
    Task DeactivateCurrentAsync();

    /// <summary>
    ///     Check if a given token is active.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>Returns true if it is active or false if it is not active.</returns>
    Task<bool> IsActiveAsync(string token);

    /// <summary>
    ///     Deactivate a given token.
    /// </summary>
    /// <param name="token"></param>
    Task DeactivateAsync(string token);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Result<UserRefreshToken> GenerateRefreshToken(User user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    Result<string> GenerateAccessToken(User user, List<string> roles);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}