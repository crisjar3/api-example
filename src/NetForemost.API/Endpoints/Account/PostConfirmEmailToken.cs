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

public class PostConfirmEmailToken : EndpointBaseAsync.WithRequest<PostConfirmEmailTokenRequest>.WithActionResult<bool>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostConfirmEmailToken> _logger;

    public PostConfirmEmailToken(IAccountService accountService, ILogger<PostConfirmEmailToken> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(bool))]
    [HttpPost(Routes.Account.PostConfirmEmailToken)]
    [SwaggerOperation(
        Summary = "Post rquest to generate token to confirm a user account.",
        Description = "Generate a token and send an email for the user to confirm their account.",
        OperationId = "Account.PostConfirmEmailToken",
        Tags = new[] { "Account" }
    )]
    public override async Task<ActionResult<bool>> HandleAsync(PostConfirmEmailTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                $"Request to consume the endpoint {Routes.Account.PostConfirmEmailToken} with the following parameters {StringHelper.ObjectToString(request)}");

            var result = await _accountService.GenerateConfirmEmailTokenAsync(request.Email);

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    $"Request to endpoint {Routes.Account.PostConfirmEmailToken} completed successfully with the following parameters {StringHelper.ObjectToString(request)}");

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(
                    $"An error has occurred when consuming the endpoint {Routes.Account.PostConfirmEmailToken} with the following parameters {StringHelper.ObjectToString(request)} for the following reason {invalidError}");

                return Problem(invalidError, Routes.Account.PostConfirmEmailToken, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(
                $"An error has occurred when consuming the endpoint {Routes.Account.PostConfirmEmailToken} with the following parameters {StringHelper.ObjectToString(request)} for the following reason {error}");

            return Problem(error, Routes.Account.PostConfirmEmailToken, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(
                $"An error has occurred when consuming the endpoint {Routes.Account.PostConfirmEmailToken} with the following parameters {User.Identity.Name} for the following reason {error}");

            return Problem(error, Routes.Account.PostConfirmEmailToken, 500);
        }
    }
}