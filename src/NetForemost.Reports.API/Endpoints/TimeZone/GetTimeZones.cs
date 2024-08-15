using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.TimeZone;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.TimeZonesReport;
using NetForemost.Report.API.Endpoints;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.Reports.API.Endpoints.TimeZone
{
    public class GetTimeZones : EndpointBaseAsync.WithoutRequest.WithActionResult<IEnumerable<TimeZoneDto>>
    {
        private readonly ITimeZoneReportService _timeZoneReportService;
        private readonly ILogger<GetTimeZones> _logger;
        private readonly UserManager<User> _userManager;

        public GetTimeZones(ITimeZoneReportService timeZoneReportService, ILogger<GetTimeZones> logger, UserManager<User> userManager)
        {
            _timeZoneReportService = timeZoneReportService;
            _logger = logger;
            _userManager = userManager;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<TimeZoneDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.TimeZone.GetAllTimeZones)]
        [SwaggerOperation(
        Summary = "Get All TimeZones",
        Description = "Find catalog of timezones",
        OperationId = "TimeZone.GetTimeZones",
        Tags = new[] { "TimeZone" })
    ]
        [Authorize]

        public override async Task<ActionResult<IEnumerable<TimeZoneDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var user = _userManager.GetUserAsync(User).Result;
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.TimeZone.GetAllTimeZones, user.Email));

                var result = _timeZoneReportService.FindAllTimeZone().Result;

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.TimeZone.GetAllTimeZones, user.Email));
                    return Ok(result.Value);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.TimeZone.GetAllTimeZones, error, user.Email));

                return Problem(error, Routes.TimeZone.GetAllTimeZones, 500);

            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                return Problem(error, Routes.TimeZone.GetAllTimeZones, 500);
            }
        }
    }
}
