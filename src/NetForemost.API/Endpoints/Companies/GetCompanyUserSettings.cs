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
    public class GetCompanyUserSettings : EndpointBaseAsync.WithRequest<GetCompanyUserDetailsRequest>.WithActionResult<CompanyUserSettingsDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICompanyUserService _companyUserService;
        private readonly ILogger<GetCompanyUserSettings> _logger;
        private readonly IMapper _mapper;

        public GetCompanyUserSettings(UserManager<User> userManager, ICompanyUserService companyUserService, ILogger<GetCompanyUserSettings> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _companyUserService = companyUserService;
            _logger = logger;
            _mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(CompanyUserSettingsDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(404, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Company.GetCompanyUserDetails)]
        [SwaggerOperation(
            Summary = "Gets an existing company user",
            Description = "Obtains the details of a companyUser.",
            OperationId = "Company.GetCompanyUserDetails",
            Tags = new[] { "Company" })
        ]

        [Authorize]
        public override async Task<ActionResult<CompanyUserSettingsDto>> HandleAsync([FromRoute] GetCompanyUserDetailsRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetCompanyUserDetails, user.Id, user.Email));

                var result = await _companyUserService.GetCompanyUserDetailsAsync(user.Id, request.Id);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetCompanyUserDetails, user.Id, user.Email));

                    var mapped = _mapper.Map<CompanyUser, CompanyUserSettingsDto>(result.Value);

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.NotFound)
                {
                    var notFoundError = ErrorHelper.GetErrors(result.Errors.ToList());

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUserDetails, user.Id, notFoundError, user.Email));

                    return Problem(notFoundError, Routes.Company.GetCompanyUserDetails, 404);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUserDetails, user.Id, error, user.Email));

                return Problem(error, Routes.Company.GetCompanyUserDetails, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUserDetails, user.Id, error, user.Email));

                return Problem(error, Routes.Company.GetCompanyUserDetails, 500);
            }
        }
    }
}
