using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
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
    public class PostProjectAvatar : EndpointBaseAsync.WithRequest<PostProjectAvatarRequest>.WithActionResult<List<ProjectAvatarDto>>
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<PostProjectAvatar> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostProjectAvatar(IProjectService projectService, ILogger<PostProjectAvatar> logger, IMapper mapper, UserManager<User> userManager)
        {
            _projectService = projectService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(201, Type = typeof(List<ProjectAvatarDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPost(Routes.Project.PostProjectAvatar)]
        [SwaggerOperation(
        Summary = "Post avatar of a project.",
        Description = "Create a avatar of to an existing project.",
        OperationId = "Project.PostProjectAvatar",
        Tags = new[] { "Project" })
        ]
        [Authorize]
        public override async Task<ActionResult<List<ProjectAvatarDto>>> HandleAsync([FromForm] PostProjectAvatarRequest request, CancellationToken cancellationToken = default)
        {
            var user = User.Attributes();
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PostProjectAvatar, request, user.Email));

                var result = await _projectService.AddAvatarToProjectAsync(request.Name, request.Description, request.ProjectId, request.Images, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<ProjectAvatar>, List<ProjectAvatarDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PostProjectAvatar, request, user.Email));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectAvatar, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Project.PostProjectAvatar, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectAvatar, request, error, user.Email));

                return Problem(error, Routes.Project.PostProjectAvatar, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);
                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectAvatar, request, error, user.Email));

                return Problem(error, Routes.Project.PostProjectAvatar, 500);
            }
        }
    }
}
