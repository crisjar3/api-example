using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Account;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account;

public class GetUserSettings : EndpointBaseAsync.WithRequest<GetUserSettingsRequest>.WithActionResult<UserSettingsDto>
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly ILogger<GetUserSettings> _logger;
    private readonly IMapper _mapper;

    public GetUserSettings(IUserSettingsService userSettingsService, IMapper mapper, ILogger<GetUserSettings> logger)
    {
        _userSettingsService = userSettingsService;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(404, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(200, Type = typeof(UserSettingsDto))]
    [HttpGet(Routes.Account.GetUserSettings)]
    [SwaggerOperation(
        Summary = "Get user settings",
        Description = "Get the settings of a registered user",
        OperationId = "Account.GetUserSettings",
        Tags = new[] { "Account" }
    )]
    [Authorize]
    public override async Task<ActionResult<UserSettingsDto>> HandleAsync([FromRoute] GetUserSettingsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.GetUserSettings, request, User.Identity.Name));

            var result = await _userSettingsService.GetUserSettingsAsync(request.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.GetUserSettings, request, User.Identity.Name));

                var mapped = _mapper.Map<UserSettings, UserSettingsDto>(result.Value);

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                var notFoundError = string.Join(",", result.Errors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.GetUserSettings, request, notFoundError, User.Identity.Name));

                return Problem(notFoundError, Routes.Account.GetUserSettings, 404);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.GetUserSettings, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.GetUserSettings, 500);
        }
    }
}