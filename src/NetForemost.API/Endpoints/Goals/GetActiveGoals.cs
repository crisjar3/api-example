using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;
public class GetActiveGoals : EndpointBaseAsync.WithoutRequest.WithActionResult<List<GoalDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetActiveGoals> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetActiveGoals(IGoalService goalService, ILogger<GetActiveGoals> logger, IMapper mapper, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<GoalDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.GetActiveGoals)]
    [SwaggerOperation(
        Summary = "Get all active goals",
        Description = "Search for all active goals for the logged in user.",
        OperationId = "Goals.GetActiveGoals",
        Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<List<GoalDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.GetActiveGoals, user.Email));
            var result = await _goalService.GetActiveGoals(user.Id, user.TimeZone.Offset);

            if (result.IsSuccess)
            {

                var mapped = _mapper.Map<List<Goal>, List<GoalDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.GetActiveGoals, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.GetActiveGoals, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.GetActiveGoals, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetActiveGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetActiveGoals, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetActiveGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetActiveGoals, 500);
        }
    }
}
