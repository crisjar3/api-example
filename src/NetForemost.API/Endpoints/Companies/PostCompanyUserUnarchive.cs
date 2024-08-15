using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

namespace NetForemost.API.Endpoints.Companies;

public class PostCompanyUserUnArchive : EndpointBaseAsync.WithRequest<PostCompanyUserArchiveRequest>.WithActionResult<CompanyUserDto>
{
    protected readonly ICompanyUserService _companyUserService;
    protected readonly ILogger<PostCompanyUserUnArchive> _logger;
    protected readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostCompanyUserUnArchive(
        ICompanyUserService companyUserService,
        ILogger<PostCompanyUserUnArchive> logger,
        IMapper mapper,
        UserManager<User> userManager
    )
    {
        _companyUserService = companyUserService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(CompanyUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Company.PostCompanyUserUnarchive)]
    [SwaggerOperation(
                Summary = "Unarchives a company user.",
                Description = "Set a company user as archived to false.",
                OperationId = "Companies.PostCompanyUserUnarchived",
                Tags = new[] { "Company" })
            ]
    [Authorize]
    public override async Task<ActionResult<CompanyUserDto>> HandleAsync(PostCompanyUserArchiveRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PostCompanyUserArchive, request, user.Email));

            var result = await _companyUserService.UnarchiveCompanyUserAsync(user.Id, request.CompanyUserId); ;

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<CompanyUser, CompanyUserDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PostCompanyUserArchive, request, user.Email));

                return Created($"Created {nameof(CompanyUserDto)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostCompanyUserArchive, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PostCompanyUserArchive, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostCompanyUserArchive, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompanyUserArchive, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostCompanyUserArchive, request, error, user.Email));

            return Problem(error, Routes.Company.PostCompanyUserArchive, 500);
        }
    }
}