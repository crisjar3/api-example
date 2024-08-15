using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Cities;
using NetForemost.Core.Dtos.Cities;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Cities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Cities;
public class GetCities : EndpointBaseAsync.WithRequest<GetCitiesRequest>.WithActionResult<List<CityDto>>
{
    private readonly ICityService _cityService;
    private readonly ILogger<GetCities> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetCities(ICityService cityService, ILogger<GetCities> logger, IMapper mapper, UserManager<User> userManager)
    {
        _cityService = cityService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<CityDto>))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.City.GetCities)]
    [SwaggerOperation(
        Summary = "Get all cities.",
        Description = "Get all cities registered.",
        OperationId = "City.GetCities",
        Tags = new[] { "City" })
    ]
    public override async Task<ActionResult<List<CityDto>>> HandleAsync([FromRoute] GetCitiesRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.City.GetCities, request));

            var result = await _cityService.FindCitiesAsync(request.CountryId);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<City>, List<CityDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.City.GetCities, request));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.City.GetCities, request, invalidError));

                return Problem(invalidError, Routes.City.GetCities, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.City.GetCities, request, error));

            return Problem(error, Routes.City.GetCities, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.City.GetCities, request, error));

            return Problem(error, Routes.City.GetCities, 500);
        }
    }
}

