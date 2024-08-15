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
public class GetCompletedGoals : EndpointBaseAsync.WithRequest<GetCompletedGoalsRequest>.WithActionResult<PaginatedRecord<GetCompletedGoalDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetCompletedGoals> _logger;
    private readonly UserManager<User> _userManager;

    public GetCompletedGoals(IGoalService goalService, ILogger<GetCompletedGoals> logger, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetCompletedGoalDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.GetCompletedGoals)]
    [SwaggerOperation(
        Summary = "Get the goals completed on time",
        Description = "Search for the goal's completed on time.",
        OperationId = "Goals.GetCompletedGoals",
        Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetCompletedGoalDto>>> HandleAsync([FromQuery] GetCompletedGoalsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.GetCompletedGoals, user.Email));

            var result = await _goalService.GetCompletedGoals(user.Id, request.From, request.To, request.PerPage, request.PageNumber, user.TimeZone.Offset);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.GetCompletedGoals, user.Email));

                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetCompletedGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetCompletedGoals, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetCompletedGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetCompletedGoals, 500);
        }
    }
}
