/*
PURPOSE

WRITTEN
  18/08/2022 17:27:12 (NetForemost)
COPYRIGHT
  Copyright © 2021–2022 NaturalSlim. All Rights Reserved.
WARNING
  This software is copyrighted! Any use of this software or other software
  whose copyright is held by IntelliProp or any software derived from such
  software without the prior written consent of the copyright holder is a
  violation of federal law punishable by imprisonment, fine or both.
  IntelliProp will pay a reward of three thousand dollars ($3,000) for
  information leading to successful civil litigation or criminal conviction
  of anyone violating a copyright held by IntelliProp.
*/

using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Countries;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Countries;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Countries
{
    public class GetCountries : EndpointBaseAsync.WithoutRequest.WithActionResult<List<CountryDto>>
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<GetCountries> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetCountries(UserManager<User> userManager, IMapper mapper, ILogger<GetCountries> logger,
            ICountryService countryService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _countryService = countryService;
        }

        [ProducesResponseType(200, Type = typeof(List<CountryDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Country.GetCountries)]
        [SwaggerOperation(
            Summary = "Get all countries.",
            Description = "Get all countries registered.",
            OperationId = "Country.GetCountries",
            Tags = new[] { "Country" })
        ]
        public override async Task<ActionResult<List<CountryDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Country.GetCountries, ""));

                var result = await _countryService.FindCountriesAsync();

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<Country>, List<CountryDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Country.GetCountries, ""));

                    return Ok(mapped);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Country.GetCountries, "", error));

                return Problem(error, Routes.Country.GetCountries, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Country.GetCountries, "", error));

                return Problem(error, Routes.Country.GetCountries, 500);
            }
        }
    }
}