using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Goals;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;
public class GetLateGoals : EndpointBaseAsync.WithRequest<GetLateGoalsRequest>.WithActionResult<PaginatedRecord<GetLateGoalDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetLateGoals> _logger;
    private readonly UserManager<User> _userManager;

    public GetLateGoals(IGoalService goalService, ILogger<GetLateGoals> logger, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetLateGoalDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.GetLateGoals)]
    [SwaggerOperation(
        Summary = "Get the goals completed but late",
        Description = "Search for the goal's completed but overdue.",
        OperationId = "Goals.GetLateGoals",
        Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetLateGoalDto>>> HandleAsync([FromQuery] GetLateGoalsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.GetLateGoals, user.Email));

            var result = await _goalService.GetLateGoals(user.Id, request.From, request.To, request.PerPage, request.PageNumber, user.TimeZone.Offset);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.GetLateGoals, user.Email));

                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetLateGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetLateGoals, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetLateGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetLateGoals, 500);
        }
    }
}
