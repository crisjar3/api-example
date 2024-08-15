/*
PURPOSE

WRITTEN
  18/08/2022 17:27:12 (NetForemost)
COPYRIGHT
  Copyright © 2021–2022 NaturalSlim. All Rights Reserved.
WARNING
  This software is copyrighted! Any use of this software or other software
  whose copyright is held by IntelliProp or any software derived from such
  software without the prior written consent of the copyright holder is a
  violation of federal law punishable by imprisonment, fine or both.
  IntelliProp will pay a reward of three thousand dollars ($3,000) for
  information leading to successful civil litigation or criminal conviction
  of anyone violating a copyright held by IntelliProp.
*/

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

namespace NetForemost.API.Endpoints.Companies;

public class GetCompany : EndpointBaseAsync.WithRequest<GetCompanyRequest>.WithActionResult<CompanyDto>
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<GetCompany> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetCompany(UserManager<User> userManager, IMapper mapper, ILogger<GetCompany> logger,
        ICompanyService companyService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _companyService = companyService;
    }

    [ProducesResponseType(200, Type = typeof(CompanyDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Company.GetCompany)]
    [SwaggerOperation(
        Summary = "Get a existing company.",
        Description = "Get the specified company of the logged in user.",
        OperationId = "Company.GetCompany",
        Tags = new[] { "Company" })
    ]
    [Authorize]
    public override async Task<ActionResult<CompanyDto>> HandleAsync([FromQuery]GetCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Company.GetCompany, "", user.Email));

            var result = await _companyService.FindCompanyAsync(request.CompanyId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<Company, CompanyDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Company.GetCompany, "", user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Company.GetCompany, "", invalidError, user.Email));

                return Problem(invalidError, Routes.Company.GetCompany, 400);
            }

            var error = string.Join(",", result.Errors);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompany, "", error, user.Email));

            return Problem(error, Routes.Company.GetCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Company.GetCompany, "", error, user.Email));

            return Problem(error, Routes.Company.GetCompany, 500);
        }
    }
}