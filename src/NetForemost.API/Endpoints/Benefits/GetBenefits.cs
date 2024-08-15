using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Benefits;
using NetForemost.Core.Dtos.Benefits;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Benefits;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Benefits;

public class GetBenefits : EndpointBaseAsync.WithRequest<GetBenefitsRequest>.WithActionResult<List<BenefitDto>>
{
    private readonly IBenefitService _benefitService;
    private readonly ILogger<GetBenefits> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetBenefits(UserManager<User> userManager, IMapper mapper, ILogger<GetBenefits> logger,
        IBenefitService benefitService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _benefitService = benefitService;
    }

    [ProducesResponseType(200, Type = typeof(List<BenefitDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Benefit.GetBenefits)]
    [SwaggerOperation(
        Summary = "Get all benefits.",
        Description = "Get all benefits, no parameter is requested since it does not perform any specific search.",
        OperationId = "Benefit.GetBenefits",
        Tags = new[] { "Benefit" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<BenefitDto>>> HandleAsync([FromRoute] GetBenefitsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Benefit.GetBenefits, request, user.Email));

            var result = await _benefitService.FindBenefitsAsync(request.CompanyId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<Benefit>, List<BenefitDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Benefit.GetBenefits, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Benefit.GetBenefits, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Benefit.GetBenefits, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Benefit.GetBenefits, request, error, user.Email));

            return Problem(error, Routes.Benefit.GetBenefits, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Benefit.GetBenefits, request, error, user.Email));

            return Problem(error, Routes.Benefit.GetBenefits, 500);
        }
    }
}