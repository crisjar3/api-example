using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PostCompanyUserInvitation : EndpointBaseAsync.WithRequest<PostCompanyUserInvitationRequest>.WithoutResult
{
    public readonly ILogger<PostCompanyUserInvitation> _logger;
    public readonly IMapper _mapper;
    public readonly ICompanyUserInvitationService _companyUserInvitationService;

    public PostCompanyUserInvitation(ILogger<PostCompanyUserInvitation> logger, IMapper mapper, ICompanyUserInvitationService companyUserInvitationService)
    {
        _logger = logger;
        _mapper = mapper;
        _companyUserInvitationService = companyUserInvitationService;
    }

    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Company.PostTeamMemberInvitation)]
    [SwaggerOperation(
            Summary = "Post new Team Member Inivitation.",
            Description = "Post a new team member invitation to colaborate with company",
            OperationId = "Company.PostTeamMemberInvitation",
            Tags = new[] { "Company" })
        ]

    [Authorize]

    public override async Task<ActionResult<CompanyUserInvitationDto>> HandleAsync(PostCompanyUserInvitationRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PostTeamMemberInvitation, request, user.Email));

            var newCompanyUserInvitation = _mapper.Map<PostCompanyUserInvitationRequest, CompanyUserInvitation>(request);

            var result = await _companyUserInvitationService.CreateInvitationCompanyUserAsync(newCompanyUserInvitation, request.Projects, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PostTeamMemberInvitation, request, user.Email));

                return NoContent();
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMemberInvitation, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PostTeamMemberInvitation, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMemberInvitation, request, error, user.Email));

            return Problem(error, Routes.Company.PostTeamMemberInvitation, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMemberInvitation, request, error, user.Email));

            return Problem(error, Routes.Company.PostTeamMemberInvitation, 500);
        }
    }
}
