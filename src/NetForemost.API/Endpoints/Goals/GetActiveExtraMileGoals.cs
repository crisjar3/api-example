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
public class GetActiveExtraMileGoals : EndpointBaseAsync.WithoutRequest.WithActionResult<List<GoalExtraMileDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetAllGoals> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetActiveExtraMileGoals(IGoalService goalService, ILogger<GetAllGoals> logger, IMapper mapper, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<GoalExtraMileDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.GetActiveExtraMileGoals)]
    [SwaggerOperation(
        Summary = "Get active extra mile goals",
        Description = "Gets all the active extra mile goals of the logged in user",
        OperationId = "Goals.GetActiveExtraMileGoals",
        Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<List<GoalExtraMileDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.GetActiveExtraMileGoals, user.Email));

            var result = await _goalService.GetActiveExtraMileGoals(user.Id, user.TimeZone.Offset);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<GoalExtraMile>, List<GoalExtraMileDto>>(result.Value);
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.GetActiveExtraMileGoals, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.GetActiveExtraMileGoals, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.GetActiveExtraMileGoals, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetActiveExtraMileGoals, error, user.Email));

            return Problem(error, Routes.Goals.GetActiveExtraMileGoals, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.Root, error, user.Email));

            return Problem(error, Routes.Goals.Root, 500);
        }
    }
}
