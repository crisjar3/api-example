using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

public class PutCompanySettings : EndpointBaseAsync.WithRequest<PutCompanySettingsRequest>.WithActionResult<CompanySettingsDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<PutCompanySettings> _logger;
    private readonly IMapper _mapper;
    private readonly ICompanySettingsService _companySettingsService;

    public PutCompanySettings(UserManager<User> userManager, IMapper mapper, ILogger<PutCompanySettings> logger, ICompanySettingsService companySettingsService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _companySettingsService = companySettingsService;
    }

    [ProducesResponseType(200, Type = typeof(CompanySettingsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Company.PutCompanySettings)]
    [SwaggerOperation(
       Summary = "Put a existing company settings.",
       Description = "Update data of an existing company settings.",
       OperationId = "Company.PutCompanySettings",
       Tags = new[] { "Company" })
    ]

    [Authorize]
    public override async Task<ActionResult<CompanySettingsDto>> HandleAsync(PutCompanySettingsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PutCompanySettings, request, user.Email));

            CompanySettings companySettings = _mapper.Map<PutCompanySettingsRequest, CompanySettings>(request);

            var result = await _companySettingsService.UpdateCompanySettingsAsync(companySettings, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PutCompanySettings, request, user.Email));

                var mapped = _mapper.Map<CompanySettings, CompanySettingsDto>(result.Value);

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PutCompanySettings, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PutCompanySettings, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PutCompanySettings, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PutCompanySettings, 500);
        }
    }
}