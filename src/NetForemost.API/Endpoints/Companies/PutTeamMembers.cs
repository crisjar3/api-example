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

namespace NetForemost.API.Endpoints.Companies
{
    public class PutTeamMembers : EndpointBaseAsync.WithRequest<PutTeamMembersRequest>.WithActionResult<PutCompanyUserUserSettingsDto>
    {
        private readonly ICompanyUserService _companyUser;
        private readonly ILogger<PutTeamMembers> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PutTeamMembers(UserManager<User> userManager, IMapper mapper, ILogger<PutTeamMembers> logger, ICompanyUserService companyUser)
        {
            _companyUser = companyUser;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(200, Type = typeof(PutCompanyUserUserSettingsDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPut(Routes.Company.PutTeamMember)]
        [SwaggerOperation(
            Summary = "Update a existing CompaniUser",
            Description = "Update data of an CompanyUser.",
            OperationId = "Team.PutTeamMember",
            Tags = new[] { "Company" })
        ]
        [Authorize]
        public override async Task<ActionResult<PutCompanyUserUserSettingsDto>> HandleAsync(PutTeamMembersRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PutCompany, request, user.Email));

                var mappedCompanyUser = _mapper.Map<PutTeamMembersRequest, CompanyUser>(request);

                var result = await _companyUser.UpdateCompanyUserAsync(mappedCompanyUser, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<CompanyUser, PutCompanyUserUserSettingsDto>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PutTeamMember, request, user.Email));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PutTeamMember, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Company.PutTeamMember, 400);
                }

                var error = string.Join(",", result.Errors);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutTeamMember, request, error, user.Email));

                return Problem(error, Routes.Company.PutTeamMember, 500);

            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutTeamMember, request, error, user.Email));

                return Problem(error, Routes.Company.PutTeamMember, 500);
            }
        }
    }
}
