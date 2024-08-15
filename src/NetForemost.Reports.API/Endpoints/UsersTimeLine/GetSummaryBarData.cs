using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.UserTimeLineReport;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.UserTimeLineReport;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.UsersTimeLine;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.UsersTimeLine;

public class GetSummaryBarData : EndpointBaseAsync.WithRequest<GetSummaryBarDataRequest>.WithActionResult<UserTimeLineSummarysBarDto>
{
    private readonly IUsersTimeLineReport _UsersTimeLineReportService;
    private readonly ILogger<GetSummaryBarData> _logger;
    private readonly UserManager<User> _UserManager;
    public GetSummaryBarData(IUsersTimeLineReport usersTimeLineReportService, ILogger<GetSummaryBarData> logger, UserManager<User> userManager)
    {
        _UsersTimeLineReportService = usersTimeLineReportService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(UserTimeLineSummarysBarDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.UserTimeLine.GetSummary)]
    [SwaggerOperation(
    Summary = "Get summary of timeline in company.",
    Description = "Get summary Timeline of project, user or dates",
    OperationId = "UserTimeLine.GetSummary",
    Tags = new[] { "UserTimeLine" })
    ]

    [Authorize]
    public override async Task<ActionResult<UserTimeLineSummarysBarDto>> HandleAsync([FromQuery] GetSummaryBarDataRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.UserTimeLine.GetSummary, request, user.Email));

            var result = await _UsersTimeLineReportService.FindUsersTimeLineSummarys(request.UserIds, request.ProjectIds, request.From, request.To, request.CompanyId, request.TimeZone);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.UserTimeLine.GetSummary, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.UserTimeLine.GetSummary, request, error, user.Email));

            return Problem(error, Routes.UserTimeLine.GetSummary, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.UserTimeLine.GetSummary, request, error, user.Email));

            return Problem(error, Routes.UserTimeLine.GetSummary, 500);
        }
    }
}
