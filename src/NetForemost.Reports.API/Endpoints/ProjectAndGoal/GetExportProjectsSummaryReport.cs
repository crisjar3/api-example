using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.ProjectAndGoal;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.ProjectAndGoal;

public class GetExportProjectsSummaryReport : EndpointBaseAsync.WithRequest<GetProjectSummaryExportRequest>.WithResult<IActionResult>
{
    private readonly IProjectAndGoalReportService _projectService;
    private readonly ILogger<GetExportProjectsSummaryReport> _logger;
    private readonly UserManager<User> _userManager;

    public GetExportProjectsSummaryReport(IProjectAndGoalReportService exportService, ILogger<GetExportProjectsSummaryReport> logger, UserManager<User> userManager)
    {
        _projectService = exportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IActionResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ProjectAndGoal.GetExportProjectSummary)]
    [SwaggerOperation(
    Summary = "Export a report file of Project Summary.",
    Description = "Download a report of Projects Summary in a CSV file.",
    OperationId = "Export.ProjectSummaryReport",
    Tags = new[] { "Project" })
]
    [Authorize]
    public override async Task<IActionResult> HandleAsync([FromQuery] GetProjectSummaryExportRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ProjectAndGoal.GetExportProjectSummary, request, user.Email));
            var result = await _projectService.ExportProjectsSummaryReport(request.UserIds, request.CompanyId, request.ProjectIds, request.From, request.To, request.TimeZone);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.ProjectAndGoal.GetExportProjectSummary, request, user.Email));
                return File(result, "application/octet-stream", "projects-summary-report_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.ProjectAndGoal.GetExportProjectSummary, request, error, user.Email));

            return Problem(error, Routes.ProjectAndGoal.GetExportProjectSummary, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.ProjectAndGoal.GetExportProjectSummary, 500);
        }
    }
}