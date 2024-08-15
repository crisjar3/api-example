using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.TaskTypes;
using NetForemost.Core.Dtos.Tasks;
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

public class GetTaskTypesRecently : EndpointBaseAsync.WithRequest<GetTaskTypesRecentlyRequest>.WithActionResult<PaginatedRecord<GetTaskTypesDto>>
{
    private readonly ITaskTypeService _taskService;
    private readonly ILogger<GetTaskTypesRecently> _logger;
    private readonly UserManager<User> _userManager;

    public GetTaskTypesRecently(ITaskTypeService taskService, ILogger<GetTaskTypesRecently> logger, UserManager<User> userManager)
    {
        _taskService = taskService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetTaskTypesDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Tasks.GetTaskTypesRecently)]
    [SwaggerOperation(
        Summary = "Get all task types recently used in task creation by the user queryable",
        Description = "Show all task types recently used in task creation by the user",
        OperationId = "Tasks.GetTasksTypesRecentlyByCompanyUser",
        Tags = new[] { "Tasks" })]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetTaskTypesDto>>> HandleAsync([FromQuery] GetTaskTypesRecentlyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.GetTaskTypesRecently, request, user.Email));

            var result = await _taskService.GetTaskTypeRecentAsync(request.CompanyUserId, request.ProjectId, request.Search, request.PageNumber, request.PerPage);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.GetTaskTypesRecently, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskTypesRecently, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Tasks.GetTaskTypesRecently, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskTypesRecently, request, error, user.Email));

            return Problem(error, Routes.Tasks.GetTaskTypesRecently, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskTypesRecently, request, error, user.Email));

            return Problem(error, Routes.Tasks.GetTaskTypesRecently, 500);
        }
    }
}

