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
public class PostCustomJobRoleCategory : EndpointBaseAsync.WithRequest<PostCustomJobRoleCategoryRequest>.WithActionResult<JobRoleCategoryDto>
{
    private readonly IJobRoleService _jobRoleService;
    private readonly ILogger<PostCustomJobRoleCategory> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostCustomJobRoleCategory(UserManager<User> userManager, IMapper mapper, ILogger<PostCustomJobRoleCategory> logger,
        IJobRoleService jobRoleService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _jobRoleService = jobRoleService;
    }

    [ProducesResponseType(201, Type = typeof(JobRoleCategoryDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.JobRole.PostCustomJobRoleCategory)]
    [SwaggerOperation(
        Summary = "Post new job role category.",
        Description = "Post new job role category.",
        OperationId = "JobRole.PostCustomJobRoleCategory",
        Tags = new[] { "JobRole" })
    ]
    [Authorize]
    public override async Task<ActionResult<JobRoleCategoryDto>> HandleAsync(PostCustomJobRoleCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobRole.PostCustomJobRoleCategory, request, user.Email));

            var jobRole = _mapper.Map<PostCustomJobRoleCategoryRequest, JobRoleCategory>(request);

            var result = await _jobRoleService.CreateCustomJobRoleCategoryAsync(jobRole, user.Id);
            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<JobRoleCategory, JobRoleCategoryDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobRole.PostCustomJobRoleCategory, request, user.Email));

                return Created($"Created {nameof(JobRoleCategory)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRoleCategory, request, invalidError, user.Email));

                return Problem(invalidError, Routes.JobRole.PostCustomJobRoleCategory, 400);
            }
            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRoleCategory, request, error, user.Email));

            return Problem(error, Routes.JobRole.PostCustomJobRoleCategory, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.PostCustomJobRoleCategory, request, error, user.Email));

            return Problem(error, Routes.JobRole.PostCustomJobRoleCategory, 500);
        }
    }
}
