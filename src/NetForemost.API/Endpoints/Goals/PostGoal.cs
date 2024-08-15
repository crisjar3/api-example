using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Goals
{
    public class PostGoal : EndpointBaseAsync.WithRequest<PostGoalRequest>.WithActionResult<FindAllGoalsDto>
    {
        private readonly IGoalService _goalService;
        private readonly ILogger<Goal> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostGoal(UserManager<User> userManager, IMapper mapper, ILogger<Goal> logger,
            IGoalService goalService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _goalService = goalService;
        }

        [ProducesResponseType(201, Type = typeof(FindAllGoalsDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPost(Routes.Goals.Root)]
        [SwaggerOperation(
            Summary = "Post new goal.",
            Description = "Post a new goal in order to keep track on your NetForemost account",
            OperationId = "Goals.PostGoal",
            Tags = new[] { "Goals" })
        ]
        [Authorize]
        public override async Task<ActionResult<FindAllGoalsDto>> HandleAsync(PostGoalRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.Root, request, user.Email));

                var goal = _mapper.Map<PostGoalRequest, Goal>(request);

                var result = await _goalService.CreateGoal(goal, user.Id, user.TimeZone.Offset); ;

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<Goal, FindAllGoalsDto>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.Root, request, user.Email));

                    return Created($"Created {nameof(FindAllGoalsDto)}", mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Goals.Root, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, error, user.Email));

                return Problem(error, Routes.Goals.Root, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.Root, request, error, user.Email));

                return Problem(error, Routes.Goals.Root, 500);
            }
        }
    }
}
