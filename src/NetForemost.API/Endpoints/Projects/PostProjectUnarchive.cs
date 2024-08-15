using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

namespace NetForemost.API.Endpoints.Projects;

public class PostProjectUnarchive : EndpointBaseAsync.WithRequest<PostProjectArchiveRequest>.WithActionResult<ProjectDto>
{
    protected readonly IProjectService _ProjectUserService;
    protected readonly ILogger<PostProjectUnarchive> _logger;
    protected readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostProjectUnarchive(
        IProjectService ProjectUserService,
        ILogger<PostProjectUnarchive> logger,
        IMapper mapper,
        UserManager<User> userManager
    )
    {
        _ProjectUserService = ProjectUserService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(ProjectDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Project.PostProjectUnarchive)]
    [SwaggerOperation(
                Summary = "Archives a Project.",
                Description = "Set a Project as archived to true.",
                OperationId = "Project.PostProjectUnarchive",
                Tags = new[] { "Project" })
            ]
    [Authorize]
    public override async Task<ActionResult<ProjectDto>> HandleAsync(PostProjectArchiveRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PostProjectUnarchive, request, user.Email));

            var result = await _ProjectUserService.UnarchiveProjectAsync(user.Id, request.ProjectId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Project, ProjectDto>(result.Value);

                return Created($"Created {nameof(ProjectDto)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectUnarchive, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PostProjectUnarchive, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectUnarchive, request, error, user.Email));

            return Problem(error, Routes.Project.PostProjectUnarchive, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectUnarchive, request, error, user.Email));

            return Problem(error, Routes.Project.PostProjectUnarchive, 500);
        }
    }
}