using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Companies;

public class PutImageCompany : EndpointBaseAsync.WithRequest<PutImageCompanyRequest>.WithActionResult<bool>
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<PutCompany> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutImageCompany(UserManager<User> userManager, ILogger<PutCompany> logger,
        ICompanyService companyService)
    {
        _userManager = userManager;
        _logger = logger;
        _companyService = companyService;
    }

    [ProducesResponseType(204, Type = typeof(bool))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.Company.PutImageCompany)]
    [SwaggerOperation(
        Summary = "Put a image company exist.",
        Description = "Change of image avatar of company.",
        OperationId = "Company.PutImageCompany",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<bool>> HandleAsync(PutImageCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.PutImageCompany, request, user.Email));

            var result = await _companyService.AddImageCompany(user.Id, request.UserImageUrl, request.CompanyId);

            if (result.IsSuccess)
            {

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.PutImageCompany, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.PutImageCompany, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Company.PutImageCompany, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutImageCompany, request, error, user.Email));

            return Problem(error, Routes.Company.PutImageCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.PutImageCompany, request, error, (await _userManager.GetUserAsync(User)).Email));

            return Problem(error, Routes.Company.PutImageCompany, 500);
        }
    }
}