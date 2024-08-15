using Ardalis.Result;
using NetForemost.Core.Dtos.Authorizations;
using NetForemost.Core.Entities.AppClients;

namespace NetForemost.Core.Interfaces.Authentication;

public interface IAuthenticationService
{
    /// <summary>
    /// This method is for user login.
    /// </summary>
    /// <param name="email">The user email.</param>
    /// <param name="password">The password of user.</param>
    ///  <returns>
    /// Returns a result of login.
    /// <br />
    /// <br />
    /// IsSuccess is true if login is successfully and Value contains a AuthorizedUserDto,
    /// if IsSuccess is false you can view Errors.
    /// </returns>
    Task<Result<AuthorizedUserDto>> AuthenticateAsync(string email, string password);

    /// <summary>
    /// This method is for refresh user token.
    /// </summary>
    /// <param name="refreshToken">The user refresh token.</param>
    /// <returns>
    /// Return success or errors.
    /// </returns>
    Task<Result<AuthorizedUserDto>> RefreshTokenAsync(string email, string refreshToken);

    /// <summary>
    /// Logging out a user by deactivating tokens
    /// </summary>
    /// <param name="refreshToken">The user refresh token</param>
    /// <param name="userId">The user mame</param>
    /// <returns>Return success or errors.</returns>
    Task<Result<bool>> LogoutAsync(string refreshToken, string userName);

    /// <summary>
    /// Verify that exist an apiKey.
    /// </summary>
    /// <param name="apiKey">The access api key.</param>
    /// <returns>Return success or errors.</returns>
    Task<Result<bool>> VerifyApiKey(string apiKey);

    /// <summary>
    /// Create a new app client.
    /// </summary>
    /// <param name="appClient">The new app client.</param>
    /// <param name="userId">The user who is creating the app client.</param>
    /// <returns>Return the app client created.</returns>
    Task<Result<AppClient>> CreateAppClientAsync(AppClient appClient, string userId);
}