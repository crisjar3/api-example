using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Tasks;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Tasks;
public class GetTasks : EndpointBaseAsync.WithRequest<GetTasksRequest>.WithActionResult<PaginatedRecord<GetTasksQueryableDto>>
{
    private readonly ITaskService _taskService;
    private readonly ILogger<GetTasks> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetTasks(ITaskService taskService, ILogger<GetTasks> logger, IMapper mapper, UserManager<User> userManager)
    {
        _taskService = taskService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetTasksQueryableDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Tasks.Root)]
    [SwaggerOperation(
        Summary = "Get all tasks queryable",
        Description = "Search for all tasks or provide the search parameters needed.",
        OperationId = "Tasks.GetTasks",
        Tags = new[] { "Tasks" })]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetTasksQueryableDto>>> HandleAsync([FromQuery] GetTasksRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.Root, request, user.Email));

            var result = await _taskService.GetTasksAsync(
                userId: user.Id,
                search: request.Search,
                typeId: request.TypeId,
                goalId: request.GoalId,
                projectId: request.ProjectId,
                companyId: request.CompanyId,
                targetEndDateFrom: request.TargetEndDateFrom,
                targetEndDateTo: request.TargetEndDateTo,
                pageNumber: request.PageNumber,
                perPage: request.PerPage
            );

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.Root, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Tasks.Root, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Tasks.Root, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.Root, request, error, user.Email));

            return Problem(error, Routes.Tasks.Root, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.Root, request, error, user.Email));

            return Problem(error, Routes.Tasks.Root, 500);
        }
    }
}
