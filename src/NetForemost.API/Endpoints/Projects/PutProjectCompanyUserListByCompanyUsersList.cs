using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Projects;
public class PutProjectCompanyUserListByCompanyUsersIds : EndpointBaseAsync.WithRequest<PostProjectCompanyUserListByCompanyUsersIdsRequest>.WithoutResult
{
    private readonly IProjectService _projectService;
    private readonly ILogger<PostProject> _logger;
    private readonly UserManager<User> _userManager;

    public PutProjectCompanyUserListByCompanyUsersIds(IProjectService projectService, ILogger<PostProject> logger, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds)]
    [SwaggerOperation(
        Summary = "Updates a list of project company user.",
        Description = "Update the status of a list of project company users to active or unactive.",
        OperationId = "Project.PutProjectCompanyUserListByCompanyUsersIds",
        Tags = new[] { "Project" })
    ]
    [Authorize]
    public override async Task<ActionResult> HandleAsync(PostProjectCompanyUserListByCompanyUsersIdsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, request, user.Email));

            var result = await _projectService.UpdateCompanyUsersStatusListForProject(request.ProjectId, request.CompanyUserIdList, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, request, user.Email));

                return NoContent();
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, request, error, user.Email));

            return Problem(error, Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, 500);
        }
        catch (Exception ex)
        {

            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, request, error, user.Email));

            return Problem(error, Routes.Project.PutProjectCompanyUserListByCompanyUsersIds, 500);
        }
    }
}
