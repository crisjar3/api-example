using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.TaskTypes;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.API.Endpoints.TaskTypes;

public class PostTaskTypesDeactivate : EndpointBaseAsync.WithRequest<PostTaskTypesActivateRequest>.WithoutResult
{
    protected readonly ITaskTypeService _taskTypeService;
    protected readonly ILogger<PostTaskTypesDeactivate> _logger;
    protected readonly IMapper _mapper;
    protected readonly UserManager<User> _userManager;

    public PostTaskTypesDeactivate(
        ITaskTypeService taskTypeService,
        ILogger<PostTaskTypesDeactivate> logger,
        IMapper mapper,
        UserManager<User> userManager
    )
    {
        _taskTypeService = taskTypeService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Tasks.PostTaskTypesDeactivate)]
    [SwaggerOperation(
        Summary = "Update the active status to one or more task types.",
        Description = "POST one or several task types to update their status as active = false.",
        OperationId = "TaskType.PostTaskTypesDeactivate",
        Tags = new[] { "Tasks" })
    ]
    [Authorize]
    public override async Task<ActionResult<Result<NoContentResult>>> HandleAsync(PostTaskTypesActivateRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            _logger.LogInformation(Routes.Tasks.PostTaskTypesDeactivate, request, user.Email);

            var result = await _taskTypeService.DeactivateTaskTypesListAsync(request.TaskTypesIds, user.Id);

            
            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.PostTaskTypesDeactivate, request, user.Email));

                return NoContent();
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Tasks.PostTaskTypesDeactivate, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Tasks.PostTaskTypesDeactivate, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.PostTaskTypesDeactivate, request, error, user.Email));

            return Problem(error, Routes.Tasks.PostTaskTypesDeactivate, 500);
        }
        catch (Exception ex)
        {
            string error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.PostTaskTypesDeactivate, request, error, user.Email));
            return Problem(error, Routes.Tasks.PostTaskTypesDeactivate, 500);
        }
    }
}