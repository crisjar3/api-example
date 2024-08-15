using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.Companies.CompanyUserInvitation;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class GetCompanyUserInvitation : EndpointBaseAsync.WithRequest<GetCompanyUserInvitationRequest>.WithActionResult<PaginatedRecord<CompanyUserInvitationCompleteDto>>
{
    private readonly ILogger<GetCompanyUserInvitation> _logger;
    private readonly ICompanyUserInvitationService _companyUserInvitationService;

    public GetCompanyUserInvitation(ILogger<GetCompanyUserInvitation> logger, ICompanyUserInvitationService companyUserInvitationService)
    {
        _logger = logger;
        _companyUserInvitationService = companyUserInvitationService;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<CompanyUserInvitationCompleteDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Company.GetTeamMemberInvitation)]
    [SwaggerOperation(
        Summary = "Get company user Invitarions",
        Description = "Get all invitation to work of company",
        OperationId = "Company.GetCompanyUserInvitation",
        Tags = new[] { "Company" })
        ]

    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<CompanyUserInvitationCompleteDto>>> HandleAsync([FromQuery] GetCompanyUserInvitationRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetTeamMemberInvitation, "", user.Email));

            var result = await _companyUserInvitationService.FindInvitationByCompanyAsync(request.CompanyId, request.PerPage, request.PageNumber);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetTeamMemberInvitation, "", user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMemberInvitation, "", invalidError, user.Email));

                return Problem(invalidError, Routes.Company.GetCompany, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMemberInvitation, "", error, user.Email));

            return Problem(error, Routes.Company.GetCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMemberInvitation, "", error, user.Email));

            return Problem(error, Routes.Company.GetCompany, 500);
        }
    }
}
