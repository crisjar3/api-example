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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;

public class GetGoalStatusByCompanyId : EndpointBaseAsync.WithRequest<GetGoalStatusByCompanyRequest>.WithActionResult<List<GoalStatusDto>>
{
    private readonly IGoalStatusService _goalStatusService;
    private readonly ILogger<GetGoalStatusByCompanyId> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetGoalStatusByCompanyId(IGoalStatusService goalStatusService, ILogger<GetGoalStatusByCompanyId> logger, IMapper mapper, UserManager<User> userManager)
    {
        _goalStatusService = goalStatusService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<GoalStatusDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.GoalStatus.Root)]
    [SwaggerOperation(
    Summary = "Get all goal status by specific company.",
    Description = "Get a list of goal status.",
    OperationId = "Goals.GetGoalStatusByCompanyId",
    Tags = new[] { "GoalStatus" })]
    [Authorize]
    public override async Task<ActionResult<List<GoalStatusDto>>> HandleAsync([FromQuery] GetGoalStatusByCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.GoalStatus.Root, request, user.Email));
            var result = await _goalStatusService.GetAllGoalStatusByCompany(request.CompanyId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<GoalStatus>, List<GoalStatusDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.GoalStatus.Root, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.GoalStatus.Root, request, invalidError, user.Email));

                return Problem(invalidError, Routes.GoalStatus.Root, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.GoalStatus.Root, request, error, user.Email));

            return Problem(error, Routes.GoalStatus.Root, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.GoalStatus.Root, request, error, user.Email));

            return Problem(error, Routes.GoalStatus.Root, 500);
        }
    }
}
