using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Timers;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Timer;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Timer;
public class PostDailyMonitoringBlock : EndpointBaseAsync.WithRequest<PostDailyMonitoringBlockRequest>.WithActionResult<DailyMonitoringBlockDto>
{
    private readonly ITimerService _timerService;
    private readonly ILogger<PostDailyMonitoringBlock> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostDailyMonitoringBlock(ITimerService timerService, ILogger<PostDailyMonitoringBlock> logger, IMapper mapper, UserManager<User> userManager)
    {
        _timerService = timerService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(DailyMonitoringBlockDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Timer.PostDailyMonitoringBlock)]
    [SwaggerOperation(
        Summary = "Post a monitoring tracking.",
        Description = "Add the monitoringto block time tracking.",
        OperationId = "Timer.PostDailyMonitoringBlock",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<DailyMonitoringBlockDto>> HandleAsync(PostDailyMonitoringBlockRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.PostDailyMonitoringBlock, request, user.Email));

            var mapped = _mapper.Map<PostDailyMonitoringBlockRequest, DailyMonitoringBlock>(request);

            var result = await _timerService.CreateDailyMonitoringBlock(mapped, user.Id);

            if (result.IsSuccess)
            {
                var resultMapped = _mapper.Map<DailyMonitoringBlockDto>(result.Value);
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.PostDailyMonitoringBlock, request, user.Email));

                return Created($"Created {nameof(DailyMonitoringBlockDto)}", resultMapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyMonitoringBlock, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Timer.PostDailyMonitoringBlock, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyMonitoringBlock, request, error, user.Email));

            return Problem(error, Routes.Timer.PostDailyMonitoringBlock, 500);
        }
        catch (Exception ex)
        {

            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyMonitoringBlock, request, error, user.Email));

            return Problem(error, Routes.Timer.PostDailyMonitoringBlock, 500);
        }
    }
}

