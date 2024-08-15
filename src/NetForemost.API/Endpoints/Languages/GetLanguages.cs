using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Languages;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Interfaces.Languages;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Languages;

public class GetLanguages : EndpointBaseAsync.WithoutRequest.WithActionResult<List<LanguageDto>>
{
    private readonly ILanguageService _languageService;
    private readonly ILogger<GetLanguages> _logger;
    private readonly IMapper _mapper;

    public GetLanguages(IMapper mapper, ILogger<GetLanguages> logger, ILanguageService languageService)
    {
        _mapper = mapper;
        _logger = logger;
        _languageService = languageService;
    }

    [ProducesResponseType(200, Type = typeof(List<LanguageDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Language.GetLanguages)]
    [SwaggerOperation(
        Summary = "Get all languages.",
        Description = "Get all languages, no parameter is requested since it does not perform any specific search.",
        OperationId = "Language.GetLanguages",
        Tags = new[] { "Language" })
    ]

    public override async Task<ActionResult<List<LanguageDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Language.GetLanguages, ""));

            var result = await _languageService.FindLanguagesAsync();

            var mapped = _mapper.Map<List<Language>, List<LanguageDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Language.GetLanguages, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Language.GetLanguages, "", error));

            return Problem(error, Routes.Language.GetLanguages, 500);
        }
    }
}