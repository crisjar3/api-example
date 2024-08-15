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
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals;
public class GetAllGoalsExtraMile : EndpointBaseAsync.WithRequest<GetGoalsExtraMileRequest>.WithActionResult<List<GoalResumeDto>>
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GetAllGoalsExtraMile> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetAllGoalsExtraMile(IGoalService goalService, ILogger<GetAllGoalsExtraMile> logger, IMapper mapper, UserManager<User> userManager)
    {
        _goalService = goalService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<GoalResumeDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Goals.GetAllExtraMileGoals)]
    [SwaggerOperation(
        Summary = "Get all goals extramiles queryable",
        Description = "Search for all Goals ExtraMiles or provide the search parameters needed.",
        OperationId = "Goals.GetAllGoalsExtraMiles",
        Tags = new[] { "Goals" })]
    [Authorize]
    public override async Task<ActionResult<List<GoalResumeDto>>> HandleAsync([FromQuery] GetGoalsExtraMileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.GetAllExtraMileGoals, request, user.Email));

            var result = await _goalService.FindAllGoalsExtraMileAsync(
                user.Id,
                request.goalId,
                request.Description,
                request.ProjectId,
                request.StoryPoints,
                request.StartDateTo, request.StartDateFrom,
                request.ActualEndDateTo, request.ActualEndDateFrom,
                request.ScrumMasterId,
                request.JiraTicketId,
                request.PriorityLevel,
                request.GoalStatusId,
                user.TimeZone.Offset,
                request.PageNumber,
                request.PerPage
            );

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<PaginatedRecord<GoalExtraMile>, PaginatedRecord<GoalExtraMileResumeDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.GetAllExtraMileGoals, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.GetAllExtraMileGoals, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Goals.GetAllExtraMileGoals, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetAllExtraMileGoals, request, error, user.Email));

            return Problem(error, Routes.Goals.GetAllExtraMileGoals, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.GetAllExtraMileGoals, request, error, user.Email));

            return Problem(error, Routes.Goals.GetAllExtraMileGoals, 500);
        }
    }
}
