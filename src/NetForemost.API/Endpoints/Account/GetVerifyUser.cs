using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Account;
using NetForemost.Core.Interfaces.Account;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account
{
    public class GetVerifyUser : EndpointBaseAsync.WithRequest<GetVerifyUserRequest>.WithActionResult<bool>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<GetVerifyUser> _logger;

        public GetVerifyUser(IAccountService accountService, ILogger<GetVerifyUser> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(204, Type = typeof(bool))]
        [HttpGet(Routes.Account.GetVerifyUser)]
        [SwaggerOperation(
       Summary = "Get a verification of user",
       Description = "Verificate if email or username is valid for use the new User",
       OperationId = "Account.GetVerifyUser",
       Tags = new[] { "Account" }
        )]

        public override async Task<ActionResult<bool>> HandleAsync([FromQuery] GetVerifyUserRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.GetVerifyUser, request));

                var result = await _accountService.VarifyUserAsync(request.Email, request.UserName);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.GetVerifyUser, request));

                    return Ok(result.Value);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.GetVerifyUser, request, invalidError));

                    return Problem(invalidError, Routes.Account.GetVerifyUser, 400);
                }

                var error = string.Join(",", result.Errors);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetVerifyUser, request, error));

                return Problem(error, Routes.Account.GetVerifyUser, 500);

            }
            catch (Exception ex)
            {
                var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetVerifyUser, request, error));

                return Problem(error, Routes.Account.GetVerifyUser, 500);
            }

        }
    }
}
