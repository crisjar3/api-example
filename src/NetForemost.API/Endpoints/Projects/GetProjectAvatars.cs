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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Projects
{
    public class GetProjectAvatars : EndpointBaseAsync.WithRequest<GetProjectAvatarRequest>.WithActionResult<List<ProjectAvatarDto>>
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<GetProjectAvatars> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetProjectAvatars(IProjectService projectService, ILogger<GetProjectAvatars> logger, IMapper mapper, UserManager<User> userManager)
        {
            _projectService = projectService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(200, Type = typeof(List<ProjectAvatarDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Project.GetProjectAvatar)]
        [SwaggerOperation(
            Summary = "Get the projectAvatars.",
            Description = "Get the projectsAvatars.",
            OperationId = "Project.GetProjectAvatar",
            Tags = new[] { "Project" })]
        [Authorize]
        public override async Task<ActionResult<List<ProjectAvatarDto>>> HandleAsync([FromQuery]GetProjectAvatarRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.GetProjectAvatar, request, user.Email));

                var result = await _projectService.GetProjectAvatarsAsync(request.ProjectId);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.GetProjectAvatar, request, user.Email));

                    var mapped = _mapper.Map<List<ProjectAvatar>, List<ProjectAvatarDto>>(result.Value);

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.GetProjectAvatar, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Project.GetProjectAvatar, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.GetProjectAvatar, request, error, user.Email));

                return Problem(error, Routes.Project.GetProjectAvatar, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.GetProjectAvatar, request, error, user.Email));

                return Problem(error, Routes.Project.GetProjectAvatar, 500);
            }
        }
    }
}
