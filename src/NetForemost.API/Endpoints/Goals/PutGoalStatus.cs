using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Goals;
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

public class PutGoalStatus : EndpointBaseAsync.WithRequest<PutGoalStatusOfGoalRequest>.WithActionResult<FindAllGoalsDto>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<Goal> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutGoalStatus(UserManager<User> userManager, IMapper mapper, ILogger<Goal> logger,
            IGoalService goalService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _goalService = goalService;
    }

    [ProducesResponseType(201, Type = typeof(FindAllGoalsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Goals.PutGoalStatus)]
    [SwaggerOperation(
    Summary = "Change the status of a goal.",
    Description = "Change the status of a goal on your NetForemost account",
    OperationId = "Goals.PutGoalStatus",
    Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<FindAllGoalsDto>> HandleAsync([FromRoute] PutGoalStatusOfGoalRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.PutGoalStatus, request, user.Email));

            var result = await _goalService.UpdateStatusGoal(request.GoalId, request.Body.GoalStatusId, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Goal, FindAllGoalsDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.PutGoalStatus, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoalStatus, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.PutGoalStatus, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoalStatus, request, error, user.Email));

            return Problem(error, Routes.Goals.PutGoalStatus, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.PutGoalStatus, request, error, user.Email));

            return Problem(error, Routes.Goals.PutGoalStatus, 500);
        }
    }
}

