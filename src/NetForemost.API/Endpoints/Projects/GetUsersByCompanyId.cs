using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Projects;

public class GetUsersByCompanyId : EndpointBaseAsync.WithRequest<GetUsersByCompanyRequest>.WithActionResult<PaginatedRecord<UsersByCompanyDto>>
{
    private readonly IProjectService _projectService;
    private readonly ILogger<GetUsersByCompanyId> _logger;
    private readonly UserManager<User> _UserManager;
    public GetUsersByCompanyId(IProjectService projectService, ILogger<GetUsersByCompanyId> logger, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _UserManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<UsersByCompanyDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Project.GetUsersByProjectCompany)]
    [SwaggerOperation(
    Summary = "Get all users of a company and project.",
    Description = "Get all users by company and indicate if belong to the project.",
    OperationId = "Project.GetUsersOfCompanyAndProject",
    Tags = new[] { "Project" })
    ]

    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<UsersByCompanyDto>>> HandleAsync([FromQuery] GetUsersByCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _UserManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.GetUsersByProjectCompany, request, user.Email));

            var result = await _projectService.GetUsersByCompanyId(request.CompanyId, request.ProjectId, request.PerPage, request.PageNumber);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.GetUsersByProjectCompany, request, user.Email));
                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.GetUsersByProjectCompany, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.GetUsersByProjectCompany, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.GetUsersByProjectCompany, request, error, user.Email));

            return Problem(error, Routes.Project.GetUsersByProjectCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            return Problem(error, Routes.Project.GetUsersByProjectCompany, 500);
        }
    }
}
