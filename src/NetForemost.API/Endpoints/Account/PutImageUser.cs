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

namespace NetForemost.API.Endpoints.Account
{
    public class PutImageUser : EndpointBaseAsync.WithRequest<PutImageUserRequest>.WithActionResult<bool>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<PutImageUser> _logger;

        public PutImageUser(IAccountService accountService, ILogger<PutImageUser> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(204, Type = typeof(bool))]
        [HttpPut(Routes.Account.PutImageUser)]
        [SwaggerOperation(
        Summary = "Put a image for user",
        Description = "Add image in avatar of user",
        OperationId = "Account.PutImageUser",
        Tags = new[] { "Account" }
    )]

        public override async Task<ActionResult<bool>> HandleAsync(PutImageUserRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PutImageUser, request));

                var result = await _accountService.AddImageToUserAsync(request.UserId, request.UserImageUrl);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PutImageUser, request));

                    return Ok(result.Value);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PutImageUser, request, invalidError));

                    return Problem(invalidError, Routes.Account.PutImageUser, 400);
                }

                var error = string.Join(",", result.Errors);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PutImageUser, request, error));

                return Problem(error, Routes.Account.PutImageUser, 500);

            }
            catch (Exception ex)
            {
                var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PutImageUser, request, error));

                return Problem(error, Routes.Account.PutImageUser, 500);
            }

        }
    }
}
