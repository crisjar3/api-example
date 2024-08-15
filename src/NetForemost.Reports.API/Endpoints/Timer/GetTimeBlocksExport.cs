using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.TimeReports;
using NetForemost.Reports.API.Requests.Timer;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Report.API.Endpoints.Timer;
public class GetTimeBlocksExport : EndpointBaseAsync.WithRequest<GetTimeBlocksByUserPerDayRequest>.WithActionResult<IEnumerable<GetDailyTimeBlockDto>>
{
    private readonly ITimeReportService _timerReportService;
    private readonly ILogger<GetTimeBlocksExport> _logger;
    private readonly UserManager<User> _userManager;

    public GetTimeBlocksExport(ITimeReportService timerReportService, ILogger<GetTimeBlocksExport> logger, UserManager<User> userManager)
    {
        _timerReportService = timerReportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<GetDailyTimeBlockDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Timer.GetTimeBlocksExport)]
    [SwaggerOperation(
        Summary = "Exports file of daily timer summary report for a user",
        Description = "Gets all the daily summary by date and by user filters.",
        OperationId = "Timer.GetTimeBlocksExport",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<IEnumerable<GetDailyTimeBlockDto>>> HandleAsync([FromQuery] GetTimeBlocksByUserPerDayRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.GetAllDailyEntriesByDateRangesPerUser, request, user.Email));

            var result = await _timerReportService.ExportTimeBlocksToCSV(request.CompanyUserId, request.TimeZone, request.From, request.To);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.GetTimeBlocksExport, request, user.Email));
                return File(result, "application/octet-stream", "users-summary-report_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.GetAllDailyEntriesByDateRangesPerUser, request, error, user.Email));

            return Problem(error, Routes.Timer.GetAllDailyEntriesByDateRangesPerUser, 500);

        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.Timer.GetAllDailyEntriesByDateRangesPerUser, 500);
        }
    }
}
