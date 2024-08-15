using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.GoalsReport;
using NetForemost.Report.API.Endpoints;
using NetForemost.Reports.API.Requests;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.Goals
{
    public class GetGoalsByProject : EndpointBaseAsync.WithRequest<GetGoalsByProjectRequest>.WithActionResult<PaginatedRecord<GetGoalByProjectDto>>
    {
        private readonly IGoalsReportService _goalsReportService;
        private readonly ILogger<GetGoalsByProject> _logger;
        private readonly UserManager<User> _userManager;

        public GetGoalsByProject(IGoalsReportService goalsReportService, ILogger<GetGoalsByProject> logger, UserManager<User> userManager)
        {
            _goalsReportService = goalsReportService;
            _logger = logger;
            _userManager = userManager;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<GetGoalByProjectDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Goal.GetGoals)]
        [SwaggerOperation(
            Summary = "Gets all goals by project and company user  Id.",
            Description = "Gets all goals by project and company user Id",
            OperationId = "Goal.GetAllGoalsByProjectAndCompanyUserId",
            Tags = new[] { "Goal" })
        ]
        [Authorize]
        public override async Task<ActionResult<PaginatedRecord<GetGoalByProjectDto>>> HandleAsync([FromQuery] GetGoalsByProjectRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goal.GetGoals, request, user.Email));

                var result = await _goalsReportService.GetGoalsByProjectAndUserCompanyId(request.CompaniesUsers, request.ProjectId, request.From, request.To, request.TimeZone, request.PerPage, request.PageNumber);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goal.GetGoals, request, user.Email));
                    return Ok(result.Value);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goal.GetGoals, request, error, user.Email));

                return Problem(error, Routes.Goal.GetGoals, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                return Problem(error, Routes.Goal.GetGoals, 500);
            }
        }
    }
}