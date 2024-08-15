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

public class PostLogin : EndpointBaseAsync.WithRequest<PostLoginRequest>.WithActionResult<AuthorizedUserDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<PostLogin> _logger;

    public PostLogin(IAuthenticationService authenticationService, ILogger<PostLogin> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(AuthorizedUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Authentication.PostLogin)]
    [SwaggerOperation(
        Summary = "Post a request to login",
        Description = "Login for the NetForemost Users, by posting a valid request.",
        OperationId = "Authentication.PostLogin",
        Tags = new[] { "Authentication" })
    ]
    public override async Task<ActionResult<AuthorizedUserDto>> HandleAsync(PostLoginRequest request, CancellationToken cancellationToken = new())
    {
        _logger.LogInformation($"Trying to login email {request.Email}");

        try
        {

            var result = await _authenticationService.AuthenticateAsync(request.Email, request.Password);

            if (result.IsSuccess)
            {
                _logger.LogInformation($"User request.Email{request.Email} has successfully logged in");

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogInformation($"Failed to login user {request.Email} for the following reason {invalidError}");

                return Problem(invalidError, Routes.Authentication.PostLogin, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError($"An error has occurred when logging in the user {request.Email} for the following reason {error}");

            return Problem(error, Routes.Authentication.PostLogin, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError($"An error has occurred when logging in the user {request.Email} for the following reason {error}");

            return Problem(error, Routes.Authentication.PostLogin, 500);
        }
    }
}