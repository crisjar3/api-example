using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests.ProjectAndGoal;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.ProjectAndGoal;

public class GetGoalData : EndpointBaseAsync.WithRequest<GetGoalDataRequest>.WithActionResult<GoalDataReportDto>
{
    private readonly IProjectAndGoalReportService _ProjectAndGoalReportService;
    private readonly ILogger<GoalDataReportDto> _logger;
    private readonly UserManager<User> _UserManager;

    public GetGoalData(IProjectAndGoalReportService projectAndGoalReportService, ILogger<GoalDataReportDto> logger, UserManager<User> userManager)
    {
        _ProjectAndGoalReportService = projectAndGoalReportService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(GoalDataReportDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ProjectAndGoal.GetGoalData)]
    [SwaggerOperation(
    Summary = "Get Goal by ID.",
    Description = "Get GoalData by Id and Users",
    OperationId = "ProjectAndGoal.GetGoalData",
    Tags = new[] { "Goal" })
    ]

    [Authorize]

    public override async Task<ActionResult<GoalDataReportDto>> HandleAsync([FromQuery] GetGoalDataRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ProjectAndGoal.GetGoalData, request, user.Email));

            var result = await _ProjectAndGoalReportService.FindGoalDataById(request.GoalId, request.TimeZone);

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
