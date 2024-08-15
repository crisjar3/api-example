using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Dtos.Projects;
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
public class GetProjects : EndpointBaseAsync.WithRequest<GetProjectRequest>.WithActionResult<List<ProjectStatusDto>>
{
    private readonly IProjectService _projectService;
    private readonly ILogger<GetProjects> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetProjects(IProjectService projectService, ILogger<GetProjects> logger, IMapper mapper, UserManager<User> userManager)
    {
        _projectService = projectService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<ProjectStatusDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Project.GetProjects)]
    [SwaggerOperation(
        Summary = "Get the projects.",
        Description = "Get the projects.",
        OperationId = "Project.GetProjects",
        Tags = new[] { "Project" })]
    [Authorize]
    public override async Task<ActionResult<List<ProjectStatusDto>>> HandleAsync([FromQuery] GetProjectRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.GetProjects, request, user.Email));

            var result = await _projectService.FindProjectsAsync(
                request.ProjectId,
                request.Name, request.Description,
                request.CompanyId, request.TechStack, request.BudgetRangeFrom,
                request.BudgetRangeTo, user.Id, request.DateStartTo, request.DateStartFrom,
                request.DateEndTo, request.DateEndFrom, request.UserId,
                request.PageNumber, request.PerPage
                );

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.GetProjects, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.GetProjects, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.GetProjects, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.GetProjects, request, error, user.Email));

            return Problem(error, Routes.Project.GetProjects, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.GetProjects, request, error, user.Email));

            return Problem(error, Routes.Project.GetProjects, 500);
        }
    }
}
