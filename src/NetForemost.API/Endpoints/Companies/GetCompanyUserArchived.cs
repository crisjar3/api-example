using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class GetCompanyUserArchived : EndpointBaseAsync.WithRequest<GetCompanyUsersRequest>.WithActionResult<PaginatedRecord<GetCompanyArchivedUsersDto>>
{
    private readonly ICompanyUserService _companyUser;
    private readonly ILogger<GetCompanyUserArchived> _logger;
    private readonly UserManager<User> _userManager;

    public GetCompanyUserArchived(ICompanyUserService companyUser, ILogger<GetCompanyUserArchived> logger, UserManager<User> userManager)
    {
        _companyUser = companyUser;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<GetCompanyArchivedUsersDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Company.GetCompanyArchivedUsers)]
    [SwaggerOperation(
        Summary = "Get all companies user archived.",
        Description = "Find users archived in a specific company.",
        OperationId = "Company.GetCompanyUsersArchived",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<GetCompanyArchivedUsersDto>>> HandleAsync([FromQuery] GetCompanyUsersRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetCompanyArchivedUsers, request, user.Email));

            var result = await _companyUser.GetArchivedCompanyUsersAsync(user.Id, request.CompanyId, request.PageNumber, request.PerPage);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetCompanyArchivedUsers, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyArchivedUsers, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.GetCompanyArchivedUsers, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyArchivedUsers, request, error, user.Email));

            return Problem(error, Routes.Company.GetCompanyArchivedUsers, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyArchivedUsers, request, error, user.Email));

            return Problem(error, Routes.Company.GetCompanyArchivedUsers, 500);
        }
    }

}