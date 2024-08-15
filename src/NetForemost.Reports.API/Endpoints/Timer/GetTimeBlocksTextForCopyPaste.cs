using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.TimeReports;
using NetForemost.Reports.API.Requests.Timer;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Report.API.Endpoints.Timer;
public class GetTimeBlocksForCopyPaste : EndpointBaseAsync.WithRequest<GetTimeBlocksByUserPerDayRequest>.WithActionResult<string>
{
    private readonly ITimeReportService _timerReportService;
    private readonly ILogger<GetTimeBlocksForCopyPaste> _logger;
    private readonly UserManager<User> _userManager;

    public GetTimeBlocksForCopyPaste(ITimeReportService timerReportService, ILogger<GetTimeBlocksForCopyPaste> logger, UserManager<User> userManager)
    {
        _timerReportService = timerReportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Timer.GetTimeBlocksForCopyPaste)]
    [SwaggerOperation(
        Summary = "Gets the list of time blocks for a user in a formatted string.",
        Description = "Gets all the daily summary by date and by user filters in a formatted string for copying and pasting it.",
        OperationId = "Timer.GetTimeBlocksForCopyPaste",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<string>> HandleAsync([FromQuery] GetTimeBlocksByUserPerDayRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.GetAllDailyEntriesByDateRangesPerUser, request, user.Email));

            var result = await _timerReportService.FormatTimeBlocksForSpreadsheet(request.CompanyUserId, request.TimeZone, request.From, request.To);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.GetTimeBlocksForCopyPaste, request, user.Email));
                return Ok(result.Value);
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
