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
public class GetBlockTimes : EndpointBaseAsync.WithRequest<GetBlockTimeByDailyEntryRequest>.WithActionResult<IEnumerable<GetBlockTimeByUserDto>>
{
    private readonly ITimeReportService _timerReportService;
    private readonly ILogger<GetBlockTimes> _logger;
    private readonly UserManager<User> _userManager;

    public GetBlockTimes(ITimeReportService timerReportService, ILogger<GetBlockTimes> logger, UserManager<User> userManager)
    {
        _timerReportService = timerReportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<GetBlockTimeByUserDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Timer.GetBlockTimes)]
    [SwaggerOperation(
        Summary = "Get block time by daily entry Id.",
        Description = "Get block time by daily entry Id.",
        OperationId = "Timer.GetBlockTimes",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<IEnumerable<GetBlockTimeByUserDto>>> HandleAsync([FromQuery] GetBlockTimeByDailyEntryRequest request, CancellationToken cancellationToken = default)
    {
        var user = _userManager.GetUserAsync(User).Result;
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.GetBlockTimes, request, user.Email));

            var result = _timerReportService.GetBlockTimeByUserIdAndDailyEntryId(request.DateDay, request.CompanyUser, request.TimeZone).Result;

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.GetBlockTimes, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.GetBlockTimes, request, error, user.Email));

            return Problem(error, Routes.Timer.GetBlockTimes, 500);

        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.Timer.GetBlockTimes, 500);
        }
    }
}
