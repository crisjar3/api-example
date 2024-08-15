using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Seniorities;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Interfaces.Seniorities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Seniorities;

public class GetSeniorities : EndpointBaseAsync.WithoutRequest.WithActionResult<List<SeniorityDto>>
{
    private readonly ISeniorityService _seniorityService;
    private readonly ILogger<GetSeniorities> _logger;
    private readonly IMapper _mapper;

    public GetSeniorities(IMapper mapper, ILogger<GetSeniorities> logger, ISeniorityService seniorityService)
    {
        _mapper = mapper;
        _logger = logger;
        _seniorityService = seniorityService;
    }

    [ProducesResponseType(200, Type = typeof(List<SeniorityDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Seniority.GetSeniorities)]
    [SwaggerOperation(
        Summary = "Get all seniorities.",
        Description = "Get all senioritys, no parameter is requested since it does not perform any specific search.",
        OperationId = "Seniority.GetSeniorities",
        Tags = new[] { "Seniority" })
    ]

    public override async Task<ActionResult<List<SeniorityDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Seniority.GetSeniorities, ""));

            var result = await _seniorityService.FindSenioritiesAsync();

            var mapped = _mapper.Map<List<Seniority>, List<SeniorityDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Seniority.GetSeniorities, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Seniority.GetSeniorities, "", error));

            return Problem(error, Routes.Seniority.GetSeniorities, 500);
        }
    }
}