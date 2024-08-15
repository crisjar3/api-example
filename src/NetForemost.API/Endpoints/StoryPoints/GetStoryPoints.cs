using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.StoryPoints;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.StoryPoints;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.StoryPoints;

public class GetStoryPoints : EndpointBaseAsync.WithoutRequest.WithActionResult<List<StoryPointDto>>
{
    private readonly IStoryPointService _storyPointService;
    private readonly ILogger<GetStoryPoints> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetStoryPoints(UserManager<User> userManager, IMapper mapper, ILogger<GetStoryPoints> logger, IStoryPointService storyPointService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _storyPointService = storyPointService;
    }

    [ProducesResponseType(200, Type = typeof(List<StoryPointDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.StoryPoints.Root)]
    [SwaggerOperation(
        Summary = "List all story points.",
        Description = "Gets all story points in the system.",
        OperationId = "StoryPoints.GetStoryPoints",
        Tags = new[] { "Story Points" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<StoryPointDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.StoryPoints.Root, ""));

            var result = await _storyPointService.FindAllAsync();

            var mapped = _mapper.Map<List<StoryPoint>, List<StoryPointDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.StoryPoints.Root, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.StoryPoints.Root, error, ""));

            return Problem(error, Routes.StoryPoints.Root, 500);
        }
    }
}