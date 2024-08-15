using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Authentication;
using NetForemost.Core.Dtos.Authorizations;
using NetForemost.Core.Interfaces.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Authentication;

public class PostRefreshToken : EndpointBaseAsync.WithRequest<PostRefreshTokenRequest>.WithActionResult<AuthorizedUserDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<PostRefreshToken> _logger;

    public PostRefreshToken(IAuthenticationService authenticationService, ILogger<PostRefreshToken> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(AuthorizedUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Authentication.PostRefreshToken)]
    [SwaggerOperation(
        Summary = "Post a request to refresh token",
        Description = "Refresh token for the NetForemost Users, by posting a valid request.",
        OperationId = "Authentication.PostRefreshToken",
        Tags = new[] { "Authentication" })
    ]

    public override async Task<ActionResult<AuthorizedUserDto>> HandleAsync(PostRefreshTokenRequest request, CancellationToken cancellationToken = new())
    {
        _logger.LogInformation($"Trying to renew token with refresh token value {request.RefreshToken}");

        try
        {
            var result = await _authenticationService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation($"Refresh token refreshed successfully");

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogInformation($"An error occurred while refreshing the access token with refresh token {request.RefreshToken}");

                return Problem(invalidError, Routes.Authentication.PostRefreshToken, 400);
            }

            if (result.Status == ResultStatus.Unauthorized)
            {
                _logger.LogInformation($"Attempted to update access with invalid token{request.RefreshToken}");

                return Problem("Attempted to update access with invalid token", Routes.Authentication.PostRefreshToken, 401);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError($"An error has occurred when refresh token {request.RefreshToken} for the following reason {error}");

            return Problem(error, Routes.Authentication.PostLogin, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError($"An error has occurred when refresh token {request.RefreshToken} for the following reason {error}");

            return Problem(error, Routes.Authentication.PostLogin, 500);
        }
    }
}