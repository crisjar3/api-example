using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
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

public class GetTeamMembers : EndpointBaseAsync.WithRequest<GetTeamMemberRequest>.WithActionResult<PaginatedRecord<UserSettingCompanyUserDto>>
{
    private readonly ICompanyUserService _companyUser;
    private readonly ILogger<GetTeamMembers> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetTeamMembers(UserManager<User> userManager, IMapper mapper, ILogger<GetTeamMembers> logger, ICompanyUserService companyUser)
    {
        _companyUser = companyUser;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(PaginatedRecord<UserSettingCompanyUserDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Company.GetTeamMember)]
    [SwaggerOperation(
        Summary = "Find members in a specific company.",
        Description = "Find members in a specific company by all filter searches.",
        OperationId = "Team.GetTeamMember",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<UserSettingCompanyUserDto>>> HandleAsync([FromQuery] GetTeamMemberRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetTeamMember, request, user.Email));

            var result = await _companyUser.FindTeamMembersAsync(request.CompanyId, request.TimeZonesIds, request.CompanyUserIds, request.isArchived, request.From, request.To, request.PageNumber, request.PerPage, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetTeamMember, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMember, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.GetTeamMember, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.GetTeamMember, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetTeamMember, request, error, user.Email));

            return Problem(error, Routes.Company.GetTeamMember, 500);
        }
    }
}