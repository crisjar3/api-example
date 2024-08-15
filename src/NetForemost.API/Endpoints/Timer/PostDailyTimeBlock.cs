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
public class PostDailyTimeBlock : EndpointBaseAsync.WithRequest<PostDailyTimeBlockRequest>.WithActionResult<DailyTimeBlockDto>
{
    private readonly ITimerService _timerService;
    private readonly ILogger<PostDailyTimeBlock> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostDailyTimeBlock(ITimerService timerService, ILogger<PostDailyTimeBlock> logger, IMapper mapper, UserManager<User> userManager)
    {
        _timerService = timerService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(DailyTimeBlockDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Timer.PostDailyTimeBlock)]
    [SwaggerOperation(
        Summary = "Post a block to tracking the time of user.",
        Description = "Create a new record of block of daily time block.",
        OperationId = "Timer.PostDailyTimeBlock",
        Tags = new[] { "Timer" })
    ]
    [Authorize]
    public override async Task<ActionResult<DailyTimeBlockDto>> HandleAsync(PostDailyTimeBlockRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Timer.PostDailyTimeBlock, request, user.Email));

            var block = _mapper.Map<PostDailyTimeBlockRequest, DailyTimeBlock>(request);

            var result = await _timerService.CreateDailyTimeBlock(user, block);

            if (result.IsSuccess)
            {
                var resultMapped = _mapper.Map<DailyTimeBlockDto>(result.Value);
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Timer.PostDailyTimeBlock, request, user.Email));

                return Created($"Created {nameof(DailyTimeBlockDto)}", resultMapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyTimeBlock, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Timer.PostDailyTimeBlock, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyTimeBlock, request, error, user.Email));

            return Problem(error, Routes.Timer.PostDailyTimeBlock, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);
            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Timer.PostDailyTimeBlock, request, error, user.Email));

            return Problem(error, Routes.Timer.PostDailyTimeBlock, 500);
        }
    }
}

