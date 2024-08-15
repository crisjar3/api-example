using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.JobOffers;
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

public class PostJobOffer : EndpointBaseAsync.WithRequest<PostJobOfferRequest>.WithActionResult<JobOfferDto>
{
    private readonly IJobOfferService _jobOfferService;
    private readonly ILogger<PostJobOffer> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostJobOffer(UserManager<User> userManager, IMapper mapper, ILogger<PostJobOffer> logger, IJobOfferService jobOfferService)
    {
        _jobOfferService = jobOfferService;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [ProducesResponseType(200, Type = typeof(List<JobOfferDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.JobOffer.PostJobOffer)]
    [SwaggerOperation(
        Summary = "Create a new job offer",
        Description = "Create a new job offer for a company.",
        OperationId = "JobOffer.PostJobOffer",
        Tags = new[] { "JobOffer" })
    ]
    [Authorize]
    public override async Task<ActionResult<JobOfferDto>> HandleAsync(PostJobOfferRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobOffer.PostJobOffer, request, user.Email));

            var jobOffer = _mapper.Map<PostJobOfferRequest, JobOffer>(request);

            var result = await _jobOfferService.CreateJobOfferAsync(jobOffer, user.Id);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobOffer.PostJobOffer, request, user.Email));

                var mapped = _mapper.Map<JobOffer, JobOfferDto>(result.Value);

                return Created($"Created {nameof(JobOffer)}", mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobOffer.PostJobOffer, request, invalidError, user.Email));

                return Problem(invalidError, Routes.JobOffer.PostJobOffer, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.PostJobOffer, request, error, user.Email));

            return Problem(error, Routes.JobOffer.PostJobOffer, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.PostJobOffer, request, error, user.Email));

            return Problem(error, Routes.JobOffer.PostJobOffer, 500);
        }
    }
}