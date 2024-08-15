using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Dashboard;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Dashboards;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Dashboard;
public class GetDashboard : EndpointBaseAsync.WithoutRequest.WithActionResult<DashboardDto>
{
    private readonly ILogger<GetDashboard> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IDashboardService _dashboardService;

    public GetDashboard(ILogger<GetDashboard> logger, IMapper mapper, UserManager<User> userManager, IDashboardService dashboardService)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _dashboardService = dashboardService;
    }

    [ProducesResponseType(200, Type = typeof(DashboardDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Dashboard.GetDashboard)]
    [SwaggerOperation(
            Summary = "Get the latest job offers, companies and projects created by users.",
            Description = "Get information on the last six weeks of projects, company users and job offers created.",
            OperationId = "Dashboard.GetDashboard",
            Tags = new[] { "Dashboard" })
        ]
    [Authorize]
    public override async Task<ActionResult<DashboardDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Dashboard.GetDashboard, "", user.Email));

            var result = await _dashboardService.GetInformationForDashboardAsync();

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Dashboard.GetDashboard, "", user.Email));

                return Ok(result.Value);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Dashboard.GetDashboard, "", error, user.Email));

            return Problem(error, Routes.Dashboard.GetDashboard, 500);

        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Dashboard.GetDashboard, "", error, ""));

            return Problem(error, Routes.Dashboard.GetDashboard, 500);
        }
    }
}

