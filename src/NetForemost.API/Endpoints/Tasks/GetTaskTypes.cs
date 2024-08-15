using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Tasks;
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
public class GetTaskTypes : EndpointBaseAsync.WithRequest<GetTaskTypesRequest>.WithActionResult<PaginatedRecord<GetTaskTypesDto>>
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly ILogger<GetTaskTypes> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetTaskTypes(ITaskTypeService taskTypeService, ILogger<GetTaskTypes> logger, IMapper mapper, UserManager<User> userManager)
    {
        _taskTypeService = taskTypeService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetTaskTypesDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Tasks.TaskTypes)]
    [SwaggerOperation(
        Summary = "Get all task types queryable",
        Description = "Search for all task types or provide the search parameters needed.",
        OperationId = "Tasks.GetTaskTypes",
        Tags = new[] { "Tasks" })]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetTaskTypesDto>>> HandleAsync([FromQuery] GetTaskTypesRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Tasks.TaskTypes, request, user.Email));

            var result = await _taskTypeService.GetTaskTypesAsync(
                userId: user.Id,
                name: request.Name,
                description: request.Description,
                projectId: request.ProjectId,
                companyId: request.CompanyId,
                pageNumber: request.PageNumber,
                perPage: request.PerPage
            );

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Tasks.TaskTypes, request, user.Email));

                return Ok(result.Value);
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
