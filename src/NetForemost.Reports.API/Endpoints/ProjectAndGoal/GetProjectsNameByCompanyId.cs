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

public class GetProjectsNameByCompanyId : EndpointBaseAsync.WithRequest<GetProjectsNameRequest>.WithActionResult<PaginatedRecord<GetProjectsNameDto>>
{
    private readonly IProjectAndGoalReportService _projectsReportService;
    private readonly ILogger<GetProjectsNameByCompanyId> _logger;
    private readonly UserManager<User> _userManager;

    public GetProjectsNameByCompanyId(IProjectAndGoalReportService projectsReportService, ILogger<GetProjectsNameByCompanyId> logger, UserManager<User> userManager)
    {
        _projectsReportService = projectsReportService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<GetProjectsNameDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ProjectAndGoal.GetProjectsByCompany)]
    [SwaggerOperation(
        Summary = "Gets all projects by specific company.",
        Description = "Gets all projects name by company Id",
        OperationId = "Projects.GetAllProjectsByCompanyId",
        Tags = new[] { "Project" })
    ]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetProjectsNameDto>>> HandleAsync([FromQuery] GetProjectsNameRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ProjectAndGoal.GetProjectsByCompany, request, user.Email));

            var result = await _projectsReportService.GetAllProjectsNameByCompanyId(request.CompanyId, request.PerPage, request.PageNumber);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.ProjectAndGoal.GetProjectsByCompany, request, user.Email));
                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.ProjectAndGoal.GetProjectsByCompany, request, error, user.Email));

            return Problem(error, Routes.ProjectAndGoal.GetProjectsByCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.ProjectAndGoal.GetProjectsByCompany, 500);
        }
    }
}