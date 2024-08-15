using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PostCompany : EndpointBaseAsync.WithRequest<PostCompanyRequest>.WithActionResult<CompanyDto>
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<PostCompany> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostCompany(UserManager<User> userManager, IMapper mapper, ILogger<PostCompany> logger,
        ICompanyService companyService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _companyService = companyService;
    }

    [ProducesResponseType(201, Type = typeof(CompanyDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Company.PostCompany)]
    [SwaggerOperation(
        Summary = "Post a new company.",
        Description = "Create a new company to manage an owner user.",
        OperationId = "Company.PostCompany",
        Tags = new[] { "Company" })
    ]

    public override async Task<ActionResult<CompanyDto>> HandleAsync(PostCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        try
        {
            if (user is null)
            {
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostCompany, request, ErrorStrings.UserNotExist, request.UserId));

                return Problem(ErrorStrings.UserNotExist, Routes.Company.PostCompany, 400);
            }

            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PostCompany, request, user.Email));

            var company = _mapper.Map<PostCompanyRequest, Company>(request);

            var result = await _companyService.CreateCompanyAsync(company, user);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Company, CompanyDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PostCompany, request, user.Email));

                return Created($"Created {nameof(Company)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostCompany, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PostCompany, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostCompany, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostCompany, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompany, 500);
        }
    }
}