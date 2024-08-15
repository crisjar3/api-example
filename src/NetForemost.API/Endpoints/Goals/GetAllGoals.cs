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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;
public class GetAllGoals : EndpointBaseAsync.WithRequest<GetGoalsRequest>.WithActionResult<List<FindAllGoalsDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetAllGoals> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetAllGoals(IGoalService goalService, ILogger<GetAllGoals> logger, IMapper mapper, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<FindAllGoalsDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.Root)]
    [SwaggerOperation(
        Summary = "Get all goals queryable",
        Description = "Search for all Goals or provide the search parameters needed.",
        OperationId = "Goals.GetAllGoals",
        Tags = new[] { "Goals" })]
    [Authorize()]
    public override async Task<ActionResult<List<FindAllGoalsDto>>> HandleAsync([FromQuery] GetGoalsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.Root, request, user.Email));

            var result = await _goalService.FindAllGoalsAsync(
                user.Id,
                request.Description,
                request.EstimatedHours,
                request.ProjectId,
                request.StoryPoints,
                request.StartDateTo, request.StartDateFrom,
                request.ActualEndDateTo, request.ActualEndDateFrom,
                request.CreationDateTo, request.CreationDateFrom,
                request.ScrumMasterId,
                request.JiraTicketId,
                request.PriorityLevel,
                user.TimeZone.Offset,
                request.GoalStatusId,
                request.CompanyId,
                request.PageNumber, request.PerPage
            );

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.Root, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.Root, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, error, user.Email));

            return Problem(error, Routes.Goals.Root, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, error, user.Email));

            return Problem(error, Routes.Goals.Root, 500);
        }
    }
}