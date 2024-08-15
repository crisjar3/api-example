using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.JobRoles;

public class GetJobRoleCategories : EndpointBaseAsync.WithRequest<GetJobRoleCategoriesRequest>.WithActionResult<List<JobRoleCategoryDto>>
{
    private readonly IJobRoleService _jobRoleService;
    private readonly ILogger<GetJobRoleCategories> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetJobRoleCategories(UserManager<User> userManager, IMapper mapper, ILogger<GetJobRoleCategories> logger,
        IJobRoleService jobRoleService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _jobRoleService = jobRoleService;
    }

    [ProducesResponseType(200, Type = typeof(List<JobRoleCategoryDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.JobRole.GetJobRoleCategories)]
    [SwaggerOperation(
        Summary = "Get all job categories with all job roles.",
        Description = "Get all job categories with all job roles, allow a the parameter company id to bring the job role category that belong to that company .",
        OperationId = "JobRole.GetJobRoleCategories",
        Tags = new[] { "JobRole" })
    ]
    public override async Task<ActionResult<List<JobRoleCategoryDto>>> HandleAsync([FromQuery] GetJobRoleCategoriesRequest request, CancellationToken cancellationToken = default)
    {

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobRole.GetJobRoleCategories, request));

            var result = await _jobRoleService.FindJobRoleCategoriesAsync(request.CompanyId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<JobRoleCategory>, List<JobRoleCategoryDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobRole.GetJobRoleCategories, request));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoleCategories, request, invalidError));

                return Problem(invalidError, Routes.JobRole.GetJobRoleCategories, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoleCategories, request, error));

            return Problem(error, Routes.JobRole.GetJobRoleCategories, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoleCategories, request, error));

            return Problem(error, Routes.JobRole.GetJobRoleCategories, 500);
        }
    }
}