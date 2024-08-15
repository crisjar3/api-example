using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PatchCompanySettings : EndpointBaseAsync.WithRequest<PatchCompanySettingsRequest>.WithActionResult<CompanySettingsDto>
{
    private readonly ICompanySettingsService _companySettingsService;
    private readonly ILogger<PatchCompanySettings> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PatchCompanySettings(
        ICompanySettingsService companySettingsService,
        ILogger<PatchCompanySettings> logger,
        IMapper mapper,
        UserManager<User> userManager
    )
    {
        _companySettingsService = companySettingsService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(CompanySettingsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPatch(Routes.Company.PatchCompanySettings)]
    [
        SwaggerOperation(
        Summary = "Update partially a company settings register.",
        Description = "Update the details of a company settings partially. <br><br>For more information about " +
                      "how to send the PATCH request, view the .NET documentation from the next link: " +
                      "<a href=\"https://learn.microsoft.com/es-es/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#resource-example\" target=\"_blank\">Microsoft Documentation about PATCH</a>.<br>" +
                      "Also you can review the documentation from <a href=\"https://jsonpatch.com\" target=\"_blank\">JSON PATCH</a> which is the library used to make PATCH requests.",
        OperationId = "Company.PatchCompanySettings",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<CompanySettingsDto>> HandleAsync(PatchCompanySettingsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PatchCompanySettings, request, user.Email));

            JsonPatchDocument<CompanySettings> patchCompanySettings = new();
            patchCompanySettings.Operations.AddRange(request.PatchCompanySettings);

            var result = await _companySettingsService.PatchCompanySettingsAsync(patchCompanySettings, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<CompanySettings, CompanySettingsDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PatchCompanySettings, request, user.Email));

                return Created($"Updated {nameof(Company)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanySettings, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PatchCompanySettings, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PatchCompanySettings, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PatchCompanySettings, 500);
        }
    }
}