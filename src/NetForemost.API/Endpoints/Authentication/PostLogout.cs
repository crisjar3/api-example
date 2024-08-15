using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Authentication;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Authentication;

public class PostLogout : EndpointBaseAsync.WithRequest<PostLogoutRequest>.WithActionResult<bool>
{
    private readonly ILogger<PostLogin> _logger;
    private readonly IAuthenticationService _authenticationService;

    public PostLogout(IAuthenticationService authenticationService, ILogger<PostLogin> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(bool))]
    [HttpPost(Routes.Authentication.PostLogout)]
    [SwaggerOperation(
        Summary = "Post close the session of the logged in user.",
        Description = "Close the session of the logged in user to end the connection..",
        OperationId = "Authentication.PostLogout",
        Tags = new[] { "Authentication" })
    ]
    [Authorize]
    public override async Task<ActionResult<bool>> HandleAsync(PostLogoutRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Authentication.PostLogout, request, User.Identity.Name));

            var result = await _authenticationService.LogoutAsync(request.RefreshToken, User.Identity.Name);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Authentication.PostLogout, request, User.Identity.Name));

                return Ok(result.Value);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Authentication.PostLogout, request, error, User.Identity.Name));

            return Problem(error, Routes.Authentication.PostLogout, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Authentication.PostLogout, request, error, User.Identity.Name));

            return Problem(error, Routes.Authentication.PostLogout, 500);
        }
    }
}