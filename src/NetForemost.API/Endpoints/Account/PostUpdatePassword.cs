using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Account;
using NetForemost.Core.Interfaces.Account;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account;

public class PostUpdatePassword : EndpointBaseAsync.WithRequest<PostChangePasswordRequest>.WithActionResult
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostUpdatePassword> _logger;

    public PostUpdatePassword(IAccountService accountService, ILogger<PostUpdatePassword> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Account.PostUpdatePassword)]
    [SwaggerOperation(
        Summary = "Post a request to change password",
        Description = "Endpoint to change the user password",
        OperationId = "Account.PostChangePassword",
        Tags = new[] { "Account" })
    ]
    [Authorize]
    public override async Task<ActionResult> HandleAsync(PostChangePasswordRequest request, CancellationToken cancellationToken = new())
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostUpdatePassword, request, User.Identity.Name));

            var result = await _accountService.UpdatePasswordAsync(User.Identity.Name, request.CurrentPassword, request.NewPassword);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostUpdatePassword, request, User.Identity.Name));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(error => error.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostUpdatePassword, request, invalidError, User.Identity.Name));

                return Problem(invalidError, Routes.Account.PostUpdatePassword, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUpdatePassword, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PostUpdatePassword, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUpdatePassword, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PostUpdatePassword, 500);
        }
    }
}