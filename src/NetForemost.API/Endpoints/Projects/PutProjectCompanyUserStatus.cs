using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Projects;
public class PutProjectCompanyUserStatus : EndpointBaseAsync.WithRequest<PutProjectCompanyUserStatusRequest>.WithActionResult<ProjectCompanyUserDto>
{
    private readonly IProjectService _projectService;
    private readonly ILogger<PutProjectCompanyUserStatus> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutProjectCompanyUserStatus(IProjectService projectService, ILogger<PutProjectCompanyUserStatus> logger, IMapper mapper, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(ProjectCompanyUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Project.PutProjectCompanyUserStatus)]
    [SwaggerOperation(
        Summary = "Update the status of user in a project.",
        Description = "Update the status of a companuy user into a project.",
        OperationId = "Project.PutProjectCompanyUserStatus",
        Tags = new[] { "Project" })
    ]
    [Authorize]
    public override async Task<ActionResult<ProjectCompanyUserDto>> HandleAsync(PutProjectCompanyUserStatusRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PutProjectCompanyUserStatus, request, user.Email));

            var result = await _projectService.UpdateProjectCompanyUserStatusAsync(request.ProjectId, request.CompanyUserId, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<ProjectCompanyUser, ProjectCompanyUserDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PutProjectCompanyUserStatus, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserStatus, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PutProjectCompanyUserStatus, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserStatus, request, error, user.Email));

            return Problem(error, Routes.Project.PutProjectCompanyUserStatus, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserStatus, request, error, user.Email));

            return Problem(error, Routes.Project.PutProjectCompanyUserStatus, 500);
        }
    }
}