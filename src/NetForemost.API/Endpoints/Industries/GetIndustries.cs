using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Industries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Interfaces.Industries;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Industries;

public class GetIndustries : EndpointBaseAsync.WithoutRequest.WithActionResult<List<IndustryDto>>
{
    private readonly IIndustryService _industryService;
    private readonly ILogger<GetIndustries> _logger;
    private readonly IMapper _mapper;

    public GetIndustries(IMapper mapper, ILogger<GetIndustries> logger, IIndustryService industryService)
    {
        _mapper = mapper;
        _logger = logger;
        _industryService = industryService;
    }

    [ProducesResponseType(200, Type = typeof(List<IndustryDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Industry.GetIndustry)]
    [SwaggerOperation(
        Summary = "Get all industries.",
        Description = "Get all industries, no parameter is requested since it does not perform any specific search.",
        OperationId = "Industry.GetIndustry",
        Tags = new[] { "Industry" })
    ]

    public override async Task<ActionResult<List<IndustryDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Industry.GetIndustry, ""));

            var result = await _industryService.FindIndustriesAsync();

            var mapped = _mapper.Map<List<Industry>, List<IndustryDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Industry.GetIndustry, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Industry.GetIndustry, "", error));

            return Problem(error, Routes.Industry.GetIndustry, 500);
        }
    }
}