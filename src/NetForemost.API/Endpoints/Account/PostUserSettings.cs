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

public class PostUserSettings : EndpointBaseAsync.WithRequest<PostUserSettingsRequest>.WithActionResult<UserSettingsDto>
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<PostUserSettings> _logger;
    private readonly IMapper _mapper;

    public PostUserSettings(IUserSettingsService userSettingsService, UserManager<User> userManager, IMapper mapper, ILogger<PostUserSettings> logger)
    {
        _userSettingsService = userSettingsService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(201, Type = typeof(UserSettingsDto))]
    [HttpPost(Routes.Account.PostUserSettings)]
    [SwaggerOperation(
        Summary = "Post user settings",
        Description = "Creates the configurations of a registered user",
        OperationId = "Account.PostUserSettings",
        Tags = new[] { "Account" }
    )]
    [Authorize]
    public override async Task<ActionResult<UserSettingsDto>> HandleAsync(PostUserSettingsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostUserSettings, request, User.Identity.Name));

            var userSettings = _mapper.Map<PostUserSettingsRequest, UserSettings>(request);

            userSettings.CreatedAt = DateTime.UtcNow;
            userSettings.CreatedBy = (await _userManager.GetUserAsync(User)).Id;

            var result = await _userSettingsService.CreateUserSettingsAsync(userSettings);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostUserSettings, request, User.Identity.Name));

                var mapped = _mapper.Map<UserSettings, UserSettingsDto>(result.Value);

                return Created("User settings created", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostUserSettings, request, invalidError, User.Identity.Name));

                return Problem(invalidError, Routes.Account.PostUserSettings, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PostUserSettings, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUserSettings, request, error, User.Identity.Name));

            return Problem(error, Routes.Account.PostUserSettings, 500);
        }
    }
}