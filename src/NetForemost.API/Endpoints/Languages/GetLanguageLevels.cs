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

public class GetLanguageLevels : EndpointBaseAsync.WithoutRequest.WithActionResult<List<LanguageLevelDto>>
{
    private readonly ILanguageService _languageService;
    private readonly ILogger<GetLanguageLevels> _logger;
    private readonly IMapper _mapper;

    public GetLanguageLevels(IMapper mapper, ILogger<GetLanguageLevels> logger, ILanguageService languageService)
    {
        _mapper = mapper;
        _logger = logger;
        _languageService = languageService;
    }

    [ProducesResponseType(200, Type = typeof(List<LanguageLevelDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Language.GetLanguageLevels)]
    [SwaggerOperation(
        Summary = "Get all language lvels.",
        Description = "Get all language levels, no parameter is requested since it does not perform any specific search.",
        OperationId = "Language.GetLanguageLevels",
        Tags = new[] { "Language" })
    ]

    public override async Task<ActionResult<List<LanguageLevelDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Language.GetLanguageLevels, ""));

            var result = await _languageService.FindLanguageLevelsAsync();

            var mapped = _mapper.Map<List<LanguageLevel>, List<LanguageLevelDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Language.GetLanguageLevels, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Language.GetLanguageLevels, "", error));

            return Problem(error, Routes.Language.GetLanguageLevels, 500);
        }
    }
}