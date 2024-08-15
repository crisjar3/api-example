using Ardalis.ApiEndpoints;
using Ardalis.Result;
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


public class PostConfirmEmail : EndpointBaseAsync.WithRequest<PostConfirmEmailRequest>.WithActionResult<bool>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostConfirmEmail> _logger;

    public PostConfirmEmail(IAccountService accountService, ILogger<PostConfirmEmail> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(200, Type = typeof(bool))]
    [HttpPost(Routes.Account.PostConfirmEmail)]
    [SwaggerOperation(
        Summary = "Post a request to confirm email",
        Description = "Endpoint to confirm the email of a registered user",
        OperationId = "Account.PostConfirmEmail",
        Tags = new[] { "Account" }
    )]
    public override async Task<ActionResult<bool>> HandleAsync(PostConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostConfirmEmail, request));

            var result = await _accountService.ConfirmEmailAsync(request.UserId, request.Token);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostConfirmEmail, request));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostConfirmEmail, request, invalidError));

                return Problem(invalidError, Routes.Account.PostConfirmEmail, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostConfirmEmail, request, error));

            return Problem(error, Routes.Account.PostConfirmEmail, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostConfirmEmail, request, error));

            return Problem(error, Routes.Account.PostConfirmEmail, 500);
        }
    }
}