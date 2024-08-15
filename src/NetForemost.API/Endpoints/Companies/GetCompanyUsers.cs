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
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies
{
    public class GetCompanyUsers : EndpointBaseAsync.WithRequest<GetCompanyUsersRequest>.WithActionResult<PaginatedRecord<SimpleCompanyUserDto>>
    {
        private readonly ICompanyUserService _companyUser;
        private readonly ILogger<GetCompanyUsers> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetCompanyUsers(UserManager<User> userManager, IMapper mapper, ILogger<GetCompanyUsers> logger, ICompanyUserService companyUser)
        {
            _companyUser = companyUser;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(200, Type = typeof(PaginatedRecord<SimpleCompanyUserDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Company.GetCompanyUsers)]
        [SwaggerOperation(
            Summary = "Find members in a specific company.",
            Description = "Find members in a specific company by all filter searches.",
            OperationId = "Company.GetCompanyUsers",
            Tags = new[] { "Company" })
        ]
        [Authorize]
        public override async Task<ActionResult<PaginatedRecord<SimpleCompanyUserDto>>> HandleAsync([FromQuery] GetCompanyUsersRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetCompanyUsers, request, user.Email));

                var result = await _companyUser.GetCompanyUsersAsync(user.Id, request.CompanyId, request.PageNumber, request.PerPage);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetCompanyUsers, request, user.Email));

                    var mapped = _mapper.Map<PaginatedRecord<CompanyUser>, PaginatedRecord<SimpleCompanyUserDto>>(result.Value);

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUsers, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Company.GetCompanyUsers, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUsers, request, error, user.Email));

                return Problem(error, Routes.Company.GetCompanyUsers, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompanyUsers, request, error, user.Email));

                return Problem(error, Routes.Company.GetCompanyUsers, 500);
            }
        }

    }
}
