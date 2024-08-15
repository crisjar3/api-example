using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Tasks;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Tasks
{
    public class PostTaskType : EndpointBaseAsync.WithRequest<PostTaskTypeRequest>.WithActionResult<TaskTypeDto>
    {
        private readonly ITaskTypeService _taskTypeService;
        private readonly ILogger<PostTaskType> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostTaskType(ITaskTypeService taskTypeService,
                            ILogger<PostTaskType> logger,
                            IMapper mapper,
                            UserManager<User> userManager)
        {
            _taskTypeService = taskTypeService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(201, Type = typeof(TaskTypeDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPost(Routes.Tasks.TaskTypes)]
        [SwaggerOperation(
            Summary = "Post a new task type.",
            Description = "Create a new task type.",
            OperationId = "Tasks.PostTaskType",
            Tags = new[] { "Tasks" })
        ]
        [Authorize]
        public override async Task<ActionResult<TaskTypeDto>> HandleAsync(PostTaskTypeRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.TaskTypes, request, user.Email));

                var taskType = _mapper.Map<PostTaskTypeRequest, TaskType>(request);

                var result = await _taskTypeService.CreateTaskTypeAsync(taskType, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<TaskType, TaskTypeDto>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.TaskTypes, request, user.Email));

                    return Created($"Created {nameof(TaskType)}", mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Tasks.TaskTypes, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Tasks.TaskTypes, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.TaskTypes, request, error, user.Email));

                return Problem(error, Routes.Tasks.TaskTypes, 500);
            }
            catch (Exception ex)
            {

                var error = ErrorHelper.GetExceptionError(ex);
                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Tasks.TaskTypes, request, error, user.Email));

                return Problem(error, Routes.Tasks.TaskTypes, 500);
            }
        }
    }
}
