using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.JobOffers;
using NetForemost.Core.Dtos.JobOffers;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.JobOffers;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.JobOffers;

public class PutJobOfferStatus : EndpointBaseAsync.WithRequest<PutJobOfferStatusRequest>.WithActionResult<ProjectCompanyUserDto>
{
    private readonly IJobOfferService _jobOfferService;
    private readonly ILogger<PutJobOfferStatus> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PutJobOfferStatus(IJobOfferService jobOfferService, ILogger<PutJobOfferStatus> logger, IMapper mapper, UserManager<User> userManager)
    {
        _jobOfferService = jobOfferService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(ProjectCompanyUserDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPut(Routes.JobOffer.PutJobOfferStatus)]
    [SwaggerOperation(
        Summary = "Update the status of a job offer.",
        Description = "Update the status of a job offer.",
        OperationId = "JobOffer.PutJobOfferStatus",
        Tags = new[] { "JobOffer" })
    ]
    [Authorize]
    public override async Task<ActionResult<ProjectCompanyUserDto>> HandleAsync(PutJobOfferStatusRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobOffer.PutJobOfferStatus, request, user.Email));

            var result = await _jobOfferService.UpdateJobOfferStatus(request.JobOfferId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<JobOffer, JobOfferDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobOffer.PutJobOfferStatus, request, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobOffer.PutJobOfferStatus, request, invalidError, user.Email));

                return Problem(invalidError, Routes.JobOffer.PutJobOfferStatus, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.PutJobOfferStatus, request, error, user.Email));

            return Problem(error, Routes.JobOffer.PutJobOfferStatus, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobOffer.PutJobOfferStatus, request, error, user.Email));

            return Problem(error, Routes.JobOffer.PutJobOfferStatus, 500);
        }
    }
}
