using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Tasks;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Tasks;
public class GetTaskById : EndpointBaseAsync.WithRequest<GetTaskByIdRequest>.WithActionResult<GetTasksQueryableDto>
{
    private readonly ITaskService _taskService;
    private readonly ILogger<GetTaskById> _logger;
    private readonly UserManager<User> _userManager;

    public GetTaskById(ITaskService taskService, ILogger<GetTaskById> logger, UserManager<User> userManager)
    {
        _taskService = taskService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(GetTasksQueryableDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Tasks.GetTaskById)]
    [SwaggerOperation(
        Summary = "Get task by id",
        Description = "Get task by id.",
        OperationId = "Tasks.GetTaskById",
        Tags = new[] { "Tasks" })]
    [Authorize]
    public override async Task<ActionResult<GetTasksQueryableDto>> HandleAsync([FromRoute] GetTaskByIdRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.GetTaskById, request, user.Email));

            var result = await _taskService.GetTasksByIdAsync(request.Id, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.GetTaskById, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskById, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Tasks.GetTaskById, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskById, request, error, user.Email));

            return Problem(error, Routes.Tasks.GetTaskById, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.GetTaskById, request, error, user.Email));

            return Problem(error, Routes.Tasks.GetTaskById, 500);
        }
    }
}
