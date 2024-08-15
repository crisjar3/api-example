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
using NetForemost.Core.Interfaces.Settings;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account;

public class PutUserSettings : EndpointBaseAsync.WithRequest<PutUserSettingsRequest>.WithActionResult<UserSettingsDto>
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<PutUserSettings> _logger;
    private readonly IMapper _mapper;

    public PutUserSettings(IUserSettingsService userSettingsService, UserManager<User> userManager, IMapper mapper, ILogger<PutUserSettings> logger)
    {
        _userSettingsService = userSettingsService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(200, Type = typeof(UserSettingsDto))]
    [HttpPut(Routes.Account.PutUserSettings)]
    [SwaggerOperation(
        Summary = "Put user settings",
        Description = "Updates the settings of a registered user",
        OperationId = "Account.PutUserSettings",
        Tags = new[] { "Account" }
    )]
    [Authorize]
    public override async Task<ActionResult<UserSettingsDto>> HandleAsync(PutUserSettingsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PutUserSettings, request, User.Identity.Name));

            var userSettings = _mapper.Map<PutUserSettingsRequest, UserSettings>(request);

            userSettings.UpdatedAt = DateTime.UtcNow;
            userSettings.UpdatedBy = (await _userManager.GetUserAsync(User)).Id;

            var result = await _userSettingsService.UpdateUserSettingsAsync(userSettings);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PutUserSettings, request, User.Identity.Name));

                var mapped = _mapper.Map<UserSettings, UserSettingsDto>(result.Value);

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PutUserSettings, request, invalidError, User.Identity.Name));

                return Problem(invalidError, Routes.Account.PutUserSettings, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PutUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PutUserSettings, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PutUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PutUserSettings, 500);
        }
    }
}