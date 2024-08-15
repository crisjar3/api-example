using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Goals;
using NetForemost.Core.Dtos.Goals;
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
    public class PostExtraMileGoal : EndpointBaseAsync.WithRequest<PostExtraMileGoalRequest>.WithActionResult<GoalExtraMileDto>
    {
        private readonly IGoalService _goalService;
        private readonly ILogger<Goal> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostExtraMileGoal(UserManager<User> userManager, IMapper mapper, ILogger<Goal> logger,
            IGoalService goalService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _goalService = goalService;
        }

        [ProducesResponseType(201, Type = typeof(GoalExtraMileDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPost(Routes.Goals.ExtraMileRoot)]
        [SwaggerOperation(
            Summary = "Post new extra mile goal.",
            Description = "Post a new extra mile goal in order to promote your growth and a great user experience to our partners",
            OperationId = "Goals.PostExtraMileGoal",
            Tags = new[] { "Goals" })
        ]
        [Authorize]
        public override async Task<ActionResult<GoalExtraMileDto>> HandleAsync(PostExtraMileGoalRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Goals.ExtraMileRoot, request, user.Email));

                var extraMileGoal = _mapper.Map<PostExtraMileGoalRequest, GoalExtraMile>(request);

                var result = await _goalService.CreateExtraMileGoal(extraMileGoal, user.Id, user.TimeZone.Offset);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<GoalExtraMile, GoalExtraMileDto>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Goals.ExtraMileRoot, request, user.Email));

                    return Created($"Created {nameof(GoalExtraMile)}", mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Goals.ExtraMileRoot, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Goals.ExtraMileRoot, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.ExtraMileRoot, request, error, user.Email));

                return Problem(error, Routes.Goals.ExtraMileRoot, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Goals.ExtraMileRoot, request, error, user.Email));

                return Problem(error, Routes.Goals.ExtraMileRoot, 500);
            }
        }
    }
}
