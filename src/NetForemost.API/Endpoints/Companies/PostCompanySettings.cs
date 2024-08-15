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

public class PostCompanySettings : EndpointBaseAsync.WithRequest<PostCompanySettingsRequest>.WithActionResult<CompanySettingsDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ICompanySettingsService _companySettingsService;
    private readonly ILogger<PostCompanySettings> _logger;
    private readonly IMapper _mapper;

    public PostCompanySettings(UserManager<User> userManager, ICompanySettingsService companySettginsService, ILogger<PostCompanySettings> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _companySettingsService = companySettginsService;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(201, Type = typeof(CompanySettingsDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Company.PostCompanySettings)]
    [SwaggerOperation(
        Summary = "Post a new company settings.",
        Description = "Create a new company settings to manage a specific company.",
        OperationId = "Company.PostCompanySettings",
        Tags = new[] { "Company" })
    ]

    [Authorize]
    public override async Task<ActionResult<CompanySettingsDto>> HandleAsync(PostCompanySettingsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PostCompanySettings, request, user.Email));

            var companySettings = _mapper.Map<PostCompanySettingsRequest, CompanySettings>(request);

            var result = await _companySettingsService.CreateCompanySettingsAsync(companySettings, user.Id, request.CompanyId);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PostCompanySettings, request, user.Email));

                var mapped = _mapper.Map<CompanySettings, CompanySettingsDto>(result.Value);

                return Created($"Created {nameof(CompanySettings)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostCompanySettings, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PostCompanySettings, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompanySettings, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanySettings, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompanySettings, 500);
        }
    }
}