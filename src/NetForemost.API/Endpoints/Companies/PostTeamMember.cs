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

public class PostTeamMember : EndpointBaseAsync.WithRequest<PostTeamMemberRequest>.WithActionResult<CompanyUserDto>
{
    private readonly ICompanyUserService _companyUser;
    private readonly ILogger<PostTeamMember> _logger;
    private readonly IMapper _mapper;

    public PostTeamMember(ICompanyUserService companyUser, ILogger<PostTeamMember> logger, IMapper mapper)
    {
        _companyUser = companyUser;
        _logger = logger;
        _mapper = mapper;
    }

    [ProducesResponseType(200, Type = typeof(CompanyUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Company.PostTeamMember)]
    [SwaggerOperation(
        Summary = "Create a new Teammate request",
        Description = "Create a new Teammate request for a company.",
        OperationId = "Team.PostTeamMember",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<CompanyUserDto>> HandleAsync(PostTeamMemberRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Attributes();

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PostTeamMember, request, user.Email));

            var newCompanyUser = _mapper.Map<PostTeamMemberRequest, CompanyUser>(request);

            var result = await _companyUser.CreateTeamMemberAsync(newCompanyUser, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PostTeamMember, request, user.Email));

                var mapped = _mapper.Map<CompanyUser, CompanyUserDto>(result.Value);

                return Created($"Created {nameof(CompanyUser)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMember, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PostTeamMember, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.PostTeamMember, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PostTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.PostTeamMember, 500);
        }
    }
}