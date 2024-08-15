using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.JobRoles;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.JobRoles;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.JobRoles;
public class PostCustomJobRole : EndpointBaseAsync.WithRequest<PostCustomJobRoleRequest>.WithActionResult<JobRoleDto>
{
    private readonly IJobRoleService _jobRoleService;
    private readonly ILogger<PostCustomJobRole> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostCustomJobRole(UserManager<User> userManager, IMapper mapper, ILogger<PostCustomJobRole> logger,
        IJobRoleService jobRoleService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _jobRoleService = jobRoleService;
    }

    [ProducesResponseType(201, Type = typeof(JobRoleDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.JobRole.PostCustomJobRole)]
    [SwaggerOperation(
        Summary = "Post new job role.",
        Description = "Post new job role.",
        OperationId = "JobRole.PostJobRole",
        Tags = new[] { "JobRole" })
    ]
    [Authorize]
    public override async Task<ActionResult<JobRoleDto>> HandleAsync(PostCustomJobRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobRole.PostCustomJobRole, request, user.Email));

            var jobRole = _mapper.Map<PostCustomJobRoleRequest, JobRole>(request);

            var result = await _jobRoleService.CreateCustomJobRoleAsync(jobRole, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<JobRole, JobRoleDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobRole.PostCustomJobRole, request, user.Email));

                return Created($"Created {nameof(JobRole)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRole, request, invalidError, user.Email));

                return Problem(invalidError, Routes.JobRole.PostCustomJobRole, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRole, request, error, user.Email));

            return Problem(error, Routes.JobRole.PostCustomJobRole, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRole, request, error, user.Email));

            return Problem(error, Routes.JobRole.PostCustomJobRole, 500);
        }
    }
}
