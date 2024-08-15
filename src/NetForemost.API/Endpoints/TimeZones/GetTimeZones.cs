/*
PURPOSE

WRITTEN
  12/09/2022 6:17:47 (NetForemost)
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
using NetForemost.Core.Dtos.TimeZone;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.TimeZones
{
    public class GetTimeZones : EndpointBaseAsync.WithoutRequest.WithActionResult<TimeZoneDto>
    {
        private readonly ILogger<GetTimeZones> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAsyncRepository<Core.Entities.TimeZones.TimeZone> _timeZoneRepository;

        public GetTimeZones(UserManager<User> userManager, IMapper mapper, ILogger<GetTimeZones> logger,
            IAsyncRepository<Core.Entities.TimeZones.TimeZone> timeZoneRepository)
        {
            _userManager = userManager;
            _timeZoneRepository = timeZoneRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(200, Type = typeof(TimeZoneDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.TimeZone.GetTimeZones)]
        [SwaggerOperation(
            Summary = "Get all time zones.",
            Description = "Get all time zones registered.",
            OperationId = "TimeZones.GetTimeZones",
            Tags = new[] { "TimeZones" })
        ]
        public override async Task<ActionResult<TimeZoneDto>> HandleAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.TimeZone.GetTimeZones, ""));

                var timeZones = await _timeZoneRepository.ListAsync();

                var mapped = _mapper.Map<List<Core.Entities.TimeZones.TimeZone>, List<TimeZoneDto>>(timeZones);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.TimeZone.GetTimeZones, ""));

                return Ok(mapped);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.TimeZone.GetTimeZones, "", error));

                return Problem(error, Routes.TimeZone.GetTimeZones, 500);
            }
        }
    }
}