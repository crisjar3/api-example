using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PatchCompanyUser : EndpointBaseAsync.WithRequest<PatchCompanyUserRequest>.WithoutResult
{
    private readonly ICompanyUserService _companyUserService;
    private readonly ILogger<PatchCompanyUser> _logger;

    public PatchCompanyUser(ICompanyUserService companyUserService, ILogger<PatchCompanyUser> logger)
    {
        _companyUserService = companyUserService;
        _logger = logger;
    }

    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPatch(Routes.Company.PatchTeamMember)]
    [
        SwaggerOperation(
        Summary = "Update partially a company user register.",
        Description = "Update the details of a company User partially. <br><br>For more information about " +
                      "how to send the PATCH request, view the .NET documentation from the next link: " +
                      "<a href=\"https://learn.microsoft.com/es-es/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#resource-example\" target=\"_blank\">Microsoft Documentation about PATCH</a>.<br>" +
                      "Also you can review the documentation from <a href=\"https://jsonpatch.com\" target=\"_blank\">JSON PATCH</a> which is the library used to make PATCH requests.",
        OperationId = "Company.PatchCompanyUser",
        Tags = new[] { "Company" })
    ]
    [Authorize]

    public override async Task<ActionResult> HandleAsync(PatchCompanyUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            JsonPatchDocument<CompanyUser> patchCompanyUser = new();

            patchCompanyUser.Operations.AddRange(request.PatchCompanyUser);

            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PatchCompanySettings, request, user.Email));

            var result = await _companyUserService.PatchCompanyUserAsync(request.CompanyUserId, patchCompanyUser, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PatchTeamMember, request, user.Email));

                return NoContent();
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PatchTeamMember, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PatchTeamMember, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.PatchTeamMember, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.PatchTeamMember, 500);
        }
    }
}
