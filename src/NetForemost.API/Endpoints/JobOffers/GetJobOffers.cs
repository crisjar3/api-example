using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.JobOffers;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.JobOffers;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.JobOffers;

public class GetJobOffers : EndpointBaseAsync.WithoutRequest.WithActionResult<List<JobOfferDto>>
{
    private readonly IJobOfferService _teamService;
    private readonly ILogger<GetJobOffers> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetJobOffers(UserManager<User> userManager, IMapper mapper, ILogger<GetJobOffers> logger, IJobOfferService teamService)
    {
        _teamService = teamService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(List<JobOfferDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.JobOffer.GetJobOffer)]
    [SwaggerOperation(
        Summary = "Get all JobOffers.",
        Description = "Search all JobOffers published that are currently active.",
        OperationId = "JobOffer.GetJobOffers",
        Tags = new[] { "JobOffer" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<JobOfferDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobOffer.GetJobOffer, user.Email));

            var result = await _teamService.FindJobOffersAsync();

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<JobOffer>, List<JobOfferDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobOffer.GetJobOffer, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobOffer.GetJobOffer, invalidError, user.Email));

                return Problem(invalidError, Routes.JobOffer.GetJobOffer, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.GetJobOffer, error, user.Email));

            return Problem(error, Routes.JobOffer.GetJobOffer, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.GetJobOffer, error, user.Email));

            return Problem(error, Routes.JobOffer.GetJobOffer, 500);
        }
    }
}