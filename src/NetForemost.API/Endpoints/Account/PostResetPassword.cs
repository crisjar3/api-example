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


public class PostResetPassword : EndpointBaseAsync.WithRequest<PostResetPasswordRequest>.WithActionResult<bool>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostResetPassword> _logger;

    public PostResetPassword(IAccountService accountService, ILogger<PostResetPassword> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(bool))]
    [HttpPost(Routes.Account.PostResetPassword)]
    [SwaggerOperation(
        Summary = "Post a request to reset password",
        Description = "Endpoint to reset password",
        OperationId = "Account.PostResetPassword",
        Tags = new[] { "Account" })
    ]
    public override async Task<ActionResult<bool>> HandleAsync(PostResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostResetPassword, request));

            var result = await _accountService.ResetPasswordAsync(request.UserId, request.Token, request.Password);


            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostResetPassword, request));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostResetPassword, request, invalidError));

                return Problem(invalidError, Routes.Account.PostResetPassword, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostResetPassword, request, error));

            return Problem(error, Routes.Account.PostResetPassword, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostResetPassword, request, error));

            return Problem(error, Routes.Account.PostResetPassword, 500);
        }
    }
}