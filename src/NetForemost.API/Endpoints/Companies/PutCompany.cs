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

public class PutCompany : EndpointBaseAsync.WithRequest<PutCompanyRequest>.WithActionResult<CompanyDto>
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<PutCompany> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutCompany(UserManager<User> userManager, IMapper mapper, ILogger<PutCompany> logger,
        ICompanyService companyService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _companyService = companyService;
    }

    [ProducesResponseType(200, Type = typeof(CompanyDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Company.PutCompany)]
    [SwaggerOperation(
        Summary = "Put a existing company.",
        Description = "Update data of an existing company.",
        OperationId = "Company.PutCompany",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<CompanyDto>> HandleAsync(PutCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PutCompany, request, user.Email));

            var company = _mapper.Map<PutCompanyRequest, Company>(request);

            var result = await _companyService.UpdateCompanyAsync(company, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Company, CompanyDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PutCompany, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PutCompany, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PutCompany, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutCompany, request, error, user.Email));

            return Problem(error, Routes.Company.PutCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutCompany, request, error, (await _userManager.GetUserAsync(User)).Email));

            return Problem(error, Routes.Company.PutCompany, 500);
        }
    }
}