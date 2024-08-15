using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Timers;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Timer;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Timer;
public class GetDailyEntryByDay : EndpointBaseAsync.WithRequest<GetDailyEntryByDayRequest>.WithActionResult<DailyEntryDto>
{
    private readonly ITimerService _timerService;
    private readonly ILogger<GetDailyEntryByDay> _logger;
    private readonly UserManager<User> _userManager;

    public GetDailyEntryByDay(ITimerService timerService, ILogger<GetDailyEntryByDay> logger, UserManager<User> userManager)
    {
        _timerService = timerService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(DailyEntryDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Timer.GetDailyEntryByDay)]
    [SwaggerOperation(
        Summary = "Get daily entry by day",
        Description = "get daily entry by day.",
        OperationId = "Timer.GetDailyEntryByDay",
        Tags = new[] { "Timer" })]
    [Authorize]
    public override async Task<ActionResult<DailyEntryDto>> HandleAsync([FromQuery] GetDailyEntryByDayRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.GetDailyEntryByDay, request, user.Email));

            var result = await _timerService.GetDailyEntryByDayAndUserId(user.Id, request.Date);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.GetDailyEntryByDay, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Timer.GetDailyEntryByDay, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Timer.GetDailyEntryByDay, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.GetDailyEntryByDay, request, error, user.Email));

            return Problem(error, Routes.Timer.GetDailyEntryByDay, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.GetDailyEntryByDay, request, error, user.Email));

            return Problem(error, Routes.Timer.GetDailyEntryByDay, 500);
        }
    }
}


