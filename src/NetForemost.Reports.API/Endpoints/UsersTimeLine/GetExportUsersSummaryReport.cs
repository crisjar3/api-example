using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.UserTimeLineReport;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.UsersTimeLine;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.UsersTimeLine;

public class GetExportUsersSummaryReport : EndpointBaseAsync.WithRequest<GetUserExportRequest>.WithResult<IActionResult>
{
    private readonly IUsersTimeLineReport _userService;
    private readonly ILogger<GetExportUsersSummaryReport> _logger;
    private readonly UserManager<User> _userManager;

    public GetExportUsersSummaryReport(IUsersTimeLineReport projectService, ILogger<GetExportUsersSummaryReport> logger, UserManager<User> userManager)
    {
        _userService = projectService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IActionResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.UserTimeLine.GetExportUserSummary)]
    [SwaggerOperation(
    Summary = "Export file of Users summary report.",
    Description = "Download a report of Users summary in a CSV file.",
    OperationId = "Export.UserSummaryReport",
    Tags = new[] { "UserTimeLine" })
]
    [Authorize]
    public override async Task<IActionResult> HandleAsync([FromQuery] GetUserExportRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.UserTimeLine.GetExportUserSummary, request, user.Email));

            var result = await _userService.ExportUsersSummaryReport(request.UserIds, request.CompanyId, request.ProjectIds, request.From, request.To, request.TimeZone);


            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.UserTimeLine.GetExportUserSummary, request, user.Email));
                return File(result, "application/octet-stream", "users-summary-report_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.UserTimeLine.GetExportUserSummary, request, error, user.Email));

            return Problem(error, Routes.UserTimeLine.GetExportUserSummary, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.UserTimeLine.GetExportUserSummary, 500);
        }
    }
}