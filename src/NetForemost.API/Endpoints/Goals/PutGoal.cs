using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Goals;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;

public class PutGoal : EndpointBaseAsync.WithRequest<PutGoalRequest>.WithActionResult<GoalDto>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<Goal> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutGoal(UserManager<User> userManager, IMapper mapper, ILogger<Goal> logger, IGoalService goalService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _goalService = goalService;
    }

    [ProducesResponseType(201, Type = typeof(GoalDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Goals.PutGoal)]
    [SwaggerOperation(
    Summary = "Update specific goal information.",
    Description = "Update goal information when it has not yet been completed already",
    OperationId = "Goals.PutGoal",
    Tags = new[] { "Goals" })
]
    [Authorize]
    public override async Task<ActionResult<GoalDto>> HandleAsync(PutGoalRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.PutGoal, request, user.Email));

            var goal = _mapper.Map<PutGoalRequest, Goal>(request);

            var result = await _goalService.UpdateGoal(goal, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Goal, GoalDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.PutGoal, request, user.Email));

                return Created($"Updated {nameof(Goal)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoal, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.PutGoal, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoal, request, error, user.Email));

            return Problem(error, Routes.Goals.PutGoal, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoal, request, error, user.Email));

            return Problem(error, Routes.Goals.PutGoal, 500);
        }
    }
}
