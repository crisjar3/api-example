using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Reports.UsersReport;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Reports.UserTimeLineReport;
using NetForemost.Report.API.Requests.UsersTimeLine;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Report.API.Endpoints.UsersTimeLine;

public class GetUsers : EndpointBaseAsync.WithRequest<GetUsersRequest>.WithActionResult<PaginatedRecord<UserSummaryDto>>
{
    private readonly IUsersTimeLineReport _UsersTimeLineReportService;
    private readonly ILogger<GetUsers> _logger;
    private readonly UserManager<User> _UserManager;
    public GetUsers(IUsersTimeLineReport usersTimeLineReportService, ILogger<GetUsers> logger, UserManager<User> userManager)
    {
        _UsersTimeLineReportService = usersTimeLineReportService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<UserSummaryDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.UserTimeLine.GetUsers)]
    [SwaggerOperation(
    Summary = "Get users of timeline in company.",
    Description = "Get users Timeline of project, user or dates",
    OperationId = "UserTimeLine.GetUsers",
    Tags = new[] { "UserTimeLine" })
    ]

    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<UserSummaryDto>>> HandleAsync([FromQuery] GetUsersRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.UserTimeLine.GetUsers, request, user.Email));

            var result = await _UsersTimeLineReportService.FindUsersSummary(request.UserIds, request.ProjectIds, request.From, request.To, request.TimeZone, request.CompanyId, request.PerPage, request.PageNumber);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.UserTimeLine.GetUsers, request, user.Email));
                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.UserTimeLine.GetUsers, invalidError, user.Email));

                return Problem(invalidError, Routes.UserTimeLine.GetUsers, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.UserTimeLine.GetUsers, request, error, user.Email));

            return Problem(error, Routes.UserTimeLine.GetUsers, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.UserTimeLine.GetUsers, 500);
        }
    }
}