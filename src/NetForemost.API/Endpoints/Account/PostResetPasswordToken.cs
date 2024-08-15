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


public class PostResetPasswordToken : EndpointBaseAsync.WithRequest<PostForgotPasswordRequest>.WithActionResult<bool>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostResetPasswordToken> _logger;

    public PostResetPasswordToken(IAccountService accountService, ILogger<PostResetPasswordToken> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(bool))]
    [HttpPost(Routes.Account.PostResetPasswordToken)]
    [SwaggerOperation(
        Summary = "Post a request to send forgot password email",
        Description = "Endpoint to send forgot password email",
        OperationId = "Account.PostResetPasswordToken",
        Tags = new[] { "Account" })
    ]
    public override async Task<ActionResult<bool>> HandleAsync(PostForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostResetPasswordToken, request));

            var result = await _accountService.GenerateResetPasswordTokenAsync(request.Email);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostResetPasswordToken, request));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostResetPasswordToken, request, invalidError));

                return Problem(invalidError, Routes.Account.PostResetPasswordToken, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostResetPasswordToken, request, error));

            return Problem(error, Routes.Account.PostResetPasswordToken, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostConfirmEmail, request, error));

            return Problem(error, Routes.Account.PostResetPasswordToken, 500);
        }
    }
}