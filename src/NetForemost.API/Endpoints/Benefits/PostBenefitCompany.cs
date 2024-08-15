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

public class PostBenefitCompany : EndpointBaseAsync.WithRequest<PostBenefitCompanyRequest>.WithActionResult<BenefitDto>
{
    private readonly IBenefitService _benefitService;
    private readonly ILogger<PostBenefitCompany> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostBenefitCompany(UserManager<User> userManager, IMapper mapper, ILogger<PostBenefitCompany> logger,
        IBenefitService benefitService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _benefitService = benefitService;
    }

    [ProducesResponseType(201, Type = typeof(List<BenefitDto>))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Benefit.PostBenefitCompany)]
    [SwaggerOperation(
        Summary = "Create a custom benefit for a company.",
        Description = "Create a new benefit customized by the company.",
        OperationId = "Benefit.PostBenefitCompany",
        Tags = new[] { "Benefit" })
    ]
    [Authorize]
    public override async Task<ActionResult<BenefitDto>> HandleAsync(PostBenefitCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Benefit.PostBenefitCompany, request, user.Email));

            var benefit = _mapper.Map<PostBenefitCompanyRequest, Benefit>(request);

            var result = await _benefitService.CreateCustomBenefit(benefit, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Benefit.PostBenefitCompany, request, user.Email));

                var mapped = _mapper.Map<Benefit, BenefitDto>(result.Value);

                return Created($"Created {nameof(Benefit)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Benefit.PostBenefitCompany, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Benefit.PostBenefitCompany, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Benefit.PostBenefitCompany, request, error, user.Email));

            return Problem(error, Routes.Benefit.PostBenefitCompany, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Benefit.PostBenefitCompany, request, error, user.Email));

            return Problem(error, Routes.Benefit.PostBenefitCompany, 500);
        }
    }
}