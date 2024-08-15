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

namespace NetForemost.API.Endpoints.Projects;
public class PostProjectCompanyUserListByCompanyUsersIds : EndpointBaseAsync.WithRequest<PostProjectCompanyUserListByCompanyUsersIdsRequest>.WithActionResult<ProjectDto>
{
    private readonly IProjectService _projectService;
    private readonly ILogger<PostProject> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostProjectCompanyUserListByCompanyUsersIds(IProjectService projectService, ILogger<PostProject> logger, IMapper mapper, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(ProjectDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds)]
    [SwaggerOperation(
    Summary = "Post several new project company users.",
    Description = "Create several new project company users.",
    OperationId = "Project.PostProjectCompanyUserListByCompanyUsersIds",
    Tags = new[] { "Project" })
]
    [Authorize]
    public override async Task<ActionResult<ProjectDto>> HandleAsync(PostProjectCompanyUserListByCompanyUsersIdsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, request, user.Email));

            var result = await _projectService.AddProjectCompanyUsersListByCompanyUserIds(request.ProjectId, request.CompanyUserIdList, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<ProjectCompanyUser>, List<ProjectCompanyUserDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, request, user.Email));

                return Created($"Created {nameof(ProjectCompanyUser)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, request, error, user.Email));

            return Problem(error, Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, 500);
        }
        catch (Exception ex)
        {

            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, request, error, user.Email));

            return Problem(error, Routes.Project.PostProjectCompanyUserListByCompanyUsersIds, 500);
        }
    }
}
