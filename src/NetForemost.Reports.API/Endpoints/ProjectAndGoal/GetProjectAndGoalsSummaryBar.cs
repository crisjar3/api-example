using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Reports.ProjectsReport;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.ProjectAndGoal;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.ProjectAndGoal;

public class GetProjectAndGoalsSummaryBar : EndpointBaseAsync.WithRequest<GetProjectAndGoalsSummaryBarRequest>.WithActionResult<AllProjectsInfoReportDto>
{
    private readonly IProjectAndGoalReportService _ProjectAndGoalReportService;
    private readonly ILogger<GetProjectAndGoalsSummaryBar> _logger;
    private readonly UserManager<User> _UserManager;

    public GetProjectAndGoalsSummaryBar(IProjectAndGoalReportService projectAndGoalReportService, ILogger<GetProjectAndGoalsSummaryBar> logger, UserManager<User> userManager)
    {
        _ProjectAndGoalReportService = projectAndGoalReportService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(AllProjectsInfoReportDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ProjectAndGoal.GetProjectAndGoalsSummary)]
    [SwaggerOperation(
   Summary = "Get project  and goals summarys in company.",
   Description = "Get summary of count goals, count projects, count hours worked",
   OperationId = "ProjectAndGoal.GetProjectAndGoalsSummary",
   Tags = new[] { "Project" })
   ]

    [Authorize]
    public override async Task<ActionResult<AllProjectsInfoReportDto>> HandleAsync([FromQuery] GetProjectAndGoalsSummaryBarRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ProjectAndGoal.GetProjectAndGoalsSummary, request, user.Email));

            var result = await _ProjectAndGoalReportService.FindAllProjectsSummary(request.CompanyUserIds, request.CompanyId, request.ProjectIds, request.From, request.To, request.TimeZone);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.ProjectAndGoal.GetGoalData, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.ProjectAndGoal.GetGoalData, request, error, user.Email));

            return Problem(error, Routes.ProjectAndGoal.GetGoalData, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.ProjectAndGoal.GetGoalData, 500);
        }
    }
}
