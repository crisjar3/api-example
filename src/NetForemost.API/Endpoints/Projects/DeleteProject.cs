using Ardalis.ApiEndpoints;
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

namespace NetForemost.API.Endpoints.Projects
{
    public class DeleteProject : EndpointBaseAsync.WithRequest<DeleteProjectRequest>.WithActionResult<ProjectSettingDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteProject> _logger;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public DeleteProject(UserManager<User> userManager, IMapper mapper, ILogger<DeleteProject> logger, IProjectService projectService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _projectService = projectService;
        }

        [ProducesResponseType(200, Type = typeof(ProjectSettingDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpDelete(Routes.Project.DeleteProject)]
        [SwaggerOperation(
           Summary = "Delete a existing project.",
           Description = "Delete a existing project.",
           OperationId = "Project.DeleteProject",
           Tags = new[] { "Project" })
        ]

        [Authorize]
        public override async Task<ActionResult<ProjectSettingDto>> HandleAsync(DeleteProjectRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.DeleteProject, request, user.Email));

                var result = await _projectService.DeleteProject(user.Id, request.ProjectId);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.DeleteProject, request, user.Email));

                    var mapped = _mapper.Map<Project, ProjectSettingDto>(result.Value);

                    return Ok(mapped);
                }

                if (result.Status == Ardalis.Result.ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.DeleteProject, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Project.DeleteProject, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.DeleteProject, request, error, user.Email));

                return Problem(error, Routes.Project.DeleteProject, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.DeleteProject, request, error, user.Email));

                return Problem(error, Routes.Project.DeleteProject, 500);
            }
        }
    }
}
