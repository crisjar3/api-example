using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

public class GetCompanySettings : EndpointBaseAsync.WithoutRequest.WithActionResult<CompanySettingsDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ICompanySettingsService _companySettingsService;
    private readonly ILogger<GetCompanySettings> _logger;
    private readonly IMapper _mapper;

    public GetCompanySettings(UserManager<User> userManager, ICompanySettingsService companySettingsService, ILogger<GetCompanySettings> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _companySettingsService = companySettingsService;
        _logger = logger;
        _mapper = mapper;
    }

    [ProducesResponseType(200, Type = typeof(CompanySettingsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(404, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Company.GetCompanySettings)]
    [SwaggerOperation(
        Summary = "Get a existing company settings.",
        Description = "Obtains the settings of a company of a user logged in with the Owner role.",
        OperationId = "Company.GetCompanySettings",
        Tags = new[] { "Company" })
    ]

    [Authorize]
    public override async Task<ActionResult<CompanySettingsDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetCompanySettings, user.Id, user.Email));

            var result = await _companySettingsService.FindCompanySettingsAsync(user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetCompanySettings, user.Id, user.Email));

                var mapped = _mapper.Map<CompanySettings, CompanySettingsDto>(result.Value);

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.NotFound)
            {
                var notFoundError = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanySettings, user.Id, notFoundError, user.Email));

                return Problem(notFoundError, Routes.Company.GetCompanySettings, 404);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanySettings, user.Id, error, user.Email));

            return Problem(error, Routes.Company.GetCompanySettings, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanySettings, user.Id, error, user.Email));

            return Problem(error, Routes.Company.GetCompanySettings, 500);
        }
    }
}