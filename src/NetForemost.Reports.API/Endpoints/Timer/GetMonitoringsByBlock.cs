using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.TimeReports;
using NetForemost.Report.API.Requests.Timer;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Report.API.Endpoints.Timer;
public class GetMonitoringsByBlock : EndpointBaseAsync.WithRequest<GetMonitoringsByBlockRequest>.WithActionResult<List<GetMonitoringByDailyTimeBlockDto>>
{
    private readonly ITimeReportService _timerReportService;
    private readonly ILogger<GetMonitoringsByBlock> _logger;
    private readonly UserManager<User> _userManager;

    public GetMonitoringsByBlock(ITimeReportService timerReportService, ILogger<GetMonitoringsByBlock> logger, UserManager<User> userManager)
    {
        _timerReportService = timerReportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<GetMonitoringByDailyTimeBlockDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Timer.GetMonitoringsByBlock)]
    [SwaggerOperation(
        Summary = "Get block time by daily entry Id.",
        Description = "Get block time by daily entry Id.",
        OperationId = "Timer.GetMonitoringsByBlock",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<GetMonitoringByDailyTimeBlockDto>>> HandleAsync([FromQuery] GetMonitoringsByBlockRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);

            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.GetMonitoringsByBlock, request, user.Email));

            var result = await _timerReportService.GetMonitoringsByBlock(user.Id, request.DailyTimeBlockId);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.GetMonitoringsByBlock, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.GetMonitoringsByBlock, request, error, user.Email));

            return Problem(error, Routes.Timer.GetMonitoringsByBlock, 500);

        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.Timer.GetMonitoringsByBlock, 500);
        }
    }
}
