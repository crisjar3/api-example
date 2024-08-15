using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PatchProject : EndpointBaseAsync.WithRequest<PatchProjectRequest>.WithoutResult
{
    private readonly IProjectService _projectService;
    private readonly ILogger<PatchProject> _logger;
    private readonly IMapper _mapper;

    public PatchProject(
        IProjectService companySettingsService,
        ILogger<PatchProject> logger,
        IMapper mapper
    )
    {
        _projectService = companySettingsService;
        _logger = logger;
        _mapper = mapper;
    }

    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPatch(Routes.Project.PatchProject)]
    [
        SwaggerOperation(
        Summary = "Update partially a project register.",
        Description = "Update the details of a project partially. <br><br>For more information about " +
                      "how to send the PATCH request, view the .NET documentation from the next link: " +
                      "<a href=\"https://learn.microsoft.com/es-es/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#resource-example\" target=\"_blank\">Microsoft Documentation about PATCH</a>.<br>" +
                      "Also you can review the documentation from <a href=\"https://jsonpatch.com\" target=\"_blank\">JSON PATCH</a> which is the library used to make PATCH requests.",
        OperationId = "Project.PatchProject",
        Tags = new[] { "Project" })
    ]
    [Authorize]
    public override async Task<ActionResult> HandleAsync(PatchProjectRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PatchProject, request, user.Email));

            JsonPatchDocument<Project> patchDocument = new();
            patchDocument.Operations.AddRange(request.PatchDocument);

            var result = await _projectService.PatchProjectAsync(request.ProjectId, patchDocument, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PatchProject, request, user.Email));

                return NoContent();
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PatchProject, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Project.PatchProject, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PatchProject, request, error, user.Email));

            return Problem(error, Routes.Project.PatchProject, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PatchProject, request, error, user.Email));

            return Problem(error, Routes.Project.PatchProject, 500);
        }
    }
}