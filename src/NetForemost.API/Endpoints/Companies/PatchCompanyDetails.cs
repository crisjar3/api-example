using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

namespace NetForemost.API.Endpoints.Companies;

// public class PatchCompanyDetails : EndpointBaseAsync.WithRequest<PatchCompanyDetailsRequest>.WithActionResult<PatchCompanyDetailsDto>
public class PatchCompanyDetails : EndpointBaseAsync.WithRequest<PatchCompanyDetailsRequest>.WithActionResult<PatchCompanyDetailsDto>
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<PatchCompanyDetails> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PatchCompanyDetails(
        ICompanyService companyService,
        ILogger<PatchCompanyDetails> logger,
        IMapper mapper,
        UserManager<User> userManager
    )
    {
        _companyService = companyService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(PatchCompanyDetailsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPatch(Routes.Company.PatchCompanyDetails)]
    [
        SwaggerOperation(
        Summary = "Update partially a company register.",
        Description = "Update the details of a company partially. <br><br>For more information about " +
                      "how to send the PATCH request, view the .NET documentation from the next link: " + 
                      "<a href=\"https://learn.microsoft.com/es-es/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#resource-example\" target=\"_blank\">Microsoft Documentation about PATCH</a>.<br>" + 
                      "Also you can review the documentation from <a href=\"https://jsonpatch.com\" target=\"_blank\">JSON PATCH</a> which is the library used to make PATCH requests.",
        OperationId = "Companies.PatchCompanyDetails",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<PatchCompanyDetailsDto>> HandleAsync(PatchCompanyDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PatchCompanyDetails, request, user.Email));

            // var company = _mapper.Map<PatchCompanyDetailsRequest, Company>(request);
            
            JsonPatchDocument<Company> patchCompany = new();
            patchCompany.Operations.AddRange(request.PatchCompany);

            var result = await _companyService.PatchCompanyDetailsAsync(user.Id, patchCompany, request.CompanyId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Company, PatchCompanyDetailsDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PatchCompanyDetails, request, user.Email));

                return Created($"Updated {nameof(Company)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanyDetails, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PatchCompanyDetails, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanyDetails, request, error, user.Email));

            return Problem(error, Routes.Company.PatchCompanyDetails, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PatchCompanyDetails, request, error, user.Email));

            return Problem(error, Routes.Company.PatchCompanyDetails, 500);
        }
    }
}