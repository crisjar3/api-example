using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
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

public class PostTalentUser : EndpointBaseAsync.WithRequest<PostTalentUserRequest>.WithActionResult<UserDto>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<PostTalentUser> _logger;
    private readonly IMapper _mapper;

    public PostTalentUser(IAccountService accountService, IMapper mapper, ILogger<PostTalentUser> logger)
    {
        _accountService = accountService;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(201, Type = typeof(UserDto))]
    [HttpPost(Routes.Account.PostTalentUser)]
    [SwaggerOperation(
        Summary = "Post a new talent user.",
        Description = "Creation of a new talent user.",
        OperationId = "Account.PostTalentUser",
        Tags = new[] { "Account" }
    )]
    public override async Task<ActionResult<UserDto>> HandleAsync(PostTalentUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostTalentUser, request));

            var talentUser = _mapper.Map<PostTalentUserRequest, User>(request);

            var result = await _accountService.CreateTalentUserAsync(talentUser, request.Password);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostTalentUser, request));

                var mapped = _mapper.Map<User, UserDto>(result.Value);

                return Created($"Created {nameof(Core.Entities.Users.User)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostTalentUser, request, invalidError));

                return Problem(invalidError, Routes.Account.PostTalentUser, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostTalentUser, request, error));

            return Problem(error, Routes.Account.PostTalentUser, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostTalentUser, request, error));

            return Problem(error, Routes.Account.PostTalentUser, 500);
        }
    }
}