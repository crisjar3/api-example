using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NetForemost.Core.Dtos.PriorityLevels;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.PriorityLevels;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.PriorityLevels;

public class GetPriorityLevels : EndpointBaseAsync.WithoutRequest.WithActionResult<List<PriorityLevelDto>>
{
    private readonly IPriorityLevelService _priorityLevelService;
    private readonly ILogger<GetPriorityLevels> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetPriorityLevels(UserManager<User> userManager, IMapper mapper, ILogger<GetPriorityLevels> logger, IPriorityLevelService priorityService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _priorityLevelService = priorityService;
    }

    [ProducesResponseType(200, Type = typeof(List<PriorityLevelDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.PriorityLevels.GetPriorityLevels)]
    [SwaggerOperation(
        Summary = "List all priority levels.",
        Description = "Gets all priority levels in the system.",
        OperationId = "PriorityLevels.GetPriorityLevels",
        Tags = new[] { "Priority Levels" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<PriorityLevelDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.PriorityLevels.GetPriorityLevels, user.Email));

            Request.Headers.TryGetValue(NameStrings.HeaderName_Language, out StringValues languageId);

            var result = await _priorityLevelService.FindAllAsync(int.Parse(languageId));

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<List<PriorityLevel>, List<PriorityLevelDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.PriorityLevels.GetPriorityLevels, user.Email));

                return Ok(mapped);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.PriorityLevels.GetPriorityLevels, "", user.Email));

                return Problem(invalidError, Routes.PriorityLevels.GetPriorityLevels, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.PriorityLevels.GetPriorityLevels, "", error, user.Email));

            return Problem(error, Routes.PriorityLevels.GetPriorityLevels, 500);

        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.PriorityLevels.GetPriorityLevels, error, user.Email));

            return Problem(error, Routes.PriorityLevels.GetPriorityLevels, 500);
        }
    }
}