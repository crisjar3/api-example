using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Reports.ProjectsReport;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.ProjectAndGoal;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.ProjectAndGoal;

public class GetProjectsSummary : EndpointBaseAsync.WithRequest<GetProjectSummaryRequest>.WithActionResult<PaginatedRecord<ProjectGeneralInfoReportDto>>
{
    private readonly IProjectAndGoalReportService _ProjectAndGoalReportService;
    private readonly ILogger<GetProjectsSummary> _logger;
    private readonly UserManager<User> _UserManager;

    public GetProjectsSummary(IProjectAndGoalReportService projectAndGoalReportService, ILogger<GetProjectsSummary> logger, UserManager<User> userManager)
    {
        _ProjectAndGoalReportService = projectAndGoalReportService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectGeneralInfoReportDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ProjectAndGoal.GetProjectSummary)]
    [SwaggerOperation(
    Summary = "Get project summarys in company.",
    Description = "Get Project with count goals and total hours worked",
    OperationId = "ProjectAndGoal.GetProject",
    Tags = new[] { "Project" })
    ]

    [Authorize]

    public override async Task<ActionResult<PaginatedRecord<ProjectGeneralInfoReportDto>>> HandleAsync([FromQuery] GetProjectSummaryRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ProjectAndGoal.GetProjectSummary, request, user.Email));

            var result = await _ProjectAndGoalReportService.FindProjectSummarys(request.UserIds, request.CompanyId, request.ProjectIds, request.From, request.To, request.TimeZone, request.PerPage, request.PageNumber);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.ProjectAndGoal.GetProjectSummary, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.ProjectAndGoal.GetProjectSummary, request, error, user.Email));

            return Problem(error, Routes.ProjectAndGoal.GetProjectSummary, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.ProjectAndGoal.GetProjectSummary, 500);
        }
    }
}

