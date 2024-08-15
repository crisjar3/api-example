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
public class PutProject : EndpointBaseAsync.WithRequest<PutProjectRequest>.WithActionResult<ProjectDto>
{
    private readonly IProjectService _projectService;
    private readonly ILogger<PutProject> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutProject(IProjectService projectService, ILogger<PutProject> logger, IMapper mapper, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(ProjectDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Project.PutProject)]
    [SwaggerOperation(
    Summary = "Put a Project.",
    Description = "Update existing project data.",
    OperationId = "Project.PutProject",
    Tags = new[] { "Project" })
]
    [Authorize]
    public override async Task<ActionResult<ProjectDto>> HandleAsync(PutProjectRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PutProject, request, user.Email));

            var project = _mapper.Map<PutProjectRequest, Project>(request);

            var result = await _projectService.UpdateProjectAsync(project, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Project, ProjectDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PutProject, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PutProject, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PutProject, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProject, request, error, user.Email));

            return Problem(error, Routes.Project.PutProject, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProject, request, error, user.Email));

            return Problem(error, Routes.Project.PutProject, 500);
        }
    }
}
