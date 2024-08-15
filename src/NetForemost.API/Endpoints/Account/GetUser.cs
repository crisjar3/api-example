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

public class GetUser : EndpointBaseAsync.WithRequest<GetUseRequest>.WithActionResult<UserDto>
{
    private readonly ILogger<GetUser> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAccountService _accountService;

    public GetUser(ILogger<GetUser> logger, IMapper mapper, UserManager<User> userManager, IAccountService accountService)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _accountService = accountService;
    }

    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(404, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Account.GetUser)]
    [SwaggerOperation(
        Summary = "Get a existing user.",
        Description = "Get the information of the user who makes the request.",
        OperationId = "Account.GetUser",
        Tags = new[] { "Account" }
    )]
    [Authorize]
    public override async Task<ActionResult<UserDto>> HandleAsync([FromRoute] GetUseRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.GetUser, request, user.Email));

            var result = await _accountService.GetUserByIdAsync(request.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<User, UserDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.GetUser, request.Id));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Unauthorized)
            {
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.GetUser, request, "Unauthorized", user.Email));

                return Problem("", Routes.Account.GetUser, 401);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                var notFoundError = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogInformation(LoggerHelper.EndpointRequestError(Routes.Account.GetUser, request, notFoundError, user.Email));

                return Problem(notFoundError, Routes.Account.GetUser, 404);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetUser, request, error, user.Email));

            return Problem(error, Routes.Account.GetUser, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetUser, user.Id, error, user.Email));

            return Problem(error, Routes.Account.GetUser, 500);
        }
    }
}