using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Tasks;
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
    public class PutTask : EndpointBaseAsync.WithRequest<PutTaskRequest>.WithActionResult<bool>
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<Core.Entities.Tasks.Task> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PutTask(ITaskService taskService,
                        ILogger<Core.Entities.Tasks.Task> logger,
                        IMapper mapper,
                        UserManager<User> userManager)
        {
            _taskService = taskService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(201, Type = typeof(bool))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPut(Routes.Tasks.Root)]
        [SwaggerOperation(
            Summary = "Update an existing task.",
            Description = "Update an existing task that fulfills all access requirements.",
            OperationId = "Tasks.PutTask",
            Tags = new[] { "Tasks" })
        ]
        [Authorize]
        public override async Task<ActionResult<bool>> HandleAsync(PutTaskRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.Root, request, user.Email));

                var task = _mapper.Map<PutTaskRequest, Core.Entities.Tasks.Task>(request);

                var result = await _taskService.UpdateTaskAsync(task, user.Id);

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
}
