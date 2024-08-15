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
    public class DeleteCompanyUser : EndpointBaseAsync.WithRequest<DeleteCompanyUserRequest>.WithActionResult<CompanyUserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteCompanyUser> _logger;
        private readonly IMapper _mapper;
        private readonly ICompanyUserService _companyUserService;

        public DeleteCompanyUser(UserManager<User> userManager, IMapper mapper, ILogger<DeleteCompanyUser> logger, ICompanyUserService companyUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _companyUserService = companyUserService;
        }

        [ProducesResponseType(200, Type = typeof(CompanyUserDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpDelete(Routes.Company.DeleteCompanyUser)]
        [SwaggerOperation(
           Summary = "Delete a existing company user.",
           Description = "Delete existing company user.",
           OperationId = "Company.DeleteCompanyUser",
           Tags = new[] { "Company" })
        ]

        [Authorize]
        public override async Task<ActionResult<CompanyUserDto>> HandleAsync(DeleteCompanyUserRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.DeleteCompanyUser, request, user.Email));

                var result = await _companyUserService.DeleteCompanyUserAsync(user.Id, request.CompanyuserId);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.DeleteCompanyUser, request, user.Email));

                    var mapped = _mapper.Map<CompanyUser, CompanyUserDto>(result.Value);

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.DeleteCompanyUser, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Company.DeleteCompanyUser, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.DeleteCompanyUser, request, error, user.Email));

                return Problem(error, Routes.Company.DeleteCompanyUser, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.DeleteCompanyUser, request, error, user.Email));

                return Problem(error, Routes.Company.DeleteCompanyUser, 500);
            }
        }
    }
}
