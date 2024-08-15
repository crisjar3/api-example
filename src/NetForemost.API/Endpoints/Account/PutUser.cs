using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Account;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Account;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account;

public class PutUser : EndpointBaseAsync.WithRequest<PutUserRequest>.WithActionResult<UserDto>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PutUser> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutUser(IAccountService accountService, UserManager<User> userManager, IMapper mapper,
        ILogger<PutUser> logger)
    {
        _accountService = accountService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [HttpPut(Routes.Account.PutUser)]
    [SwaggerOperation(
            Summary = "Put a existing user.",
            Description = "If the user is registered, they can send a request to update their data.",
            OperationId = "Authentication.PutUser",
            Tags = new[] { "Account" }
        )
    ]
    [Authorize]
    public override async Task<ActionResult<UserDto>> HandleAsync(PutUserRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);

            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PutUser, request, user.Email));

            var applicationUser = _mapper.Map<PutUserRequest, User>(request);

            applicationUser.Id = user.Id;

            var result = await _accountService.UpdateUserAsync(applicationUser);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PutUser, request, user.Email));

                var mapped = _mapper.Map<User, UserDto>(result.Value);

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PutUser, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Account.PutUser, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PutUser, request, error, user.Email));

            return Problem(error, Routes.Account.PutUser, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostConfirmEmail, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PutUser, 500);
        }
    }
}