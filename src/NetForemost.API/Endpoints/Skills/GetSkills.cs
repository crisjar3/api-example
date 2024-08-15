using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Dtos.Skills;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Skills;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Skills;

public class GetSkills : EndpointBaseAsync.WithoutRequest.WithActionResult<List<SkillDto>>
{
    private readonly ISkillService _skillService;
    private readonly ILogger<GetSkills> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetSkills(UserManager<User> userManager, IMapper mapper, ILogger<GetSkills> logger, ISkillService skillService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _skillService = skillService;
    }

    [ProducesResponseType(200, Type = typeof(List<SkillDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Skill.GetSkills)]
    [SwaggerOperation(
        Summary = "Get all skills.",
        Description = "Get all skills, no parameter is requested since it does not perform any specific search.",
        OperationId = "Skill.GetSkills",
        Tags = new[] { "Skill" })
    ]

    public override async Task<ActionResult<List<SkillDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Skill.GetSkills, ""));

            var result = await _skillService.FindSkillsAsync();

            var mapped = _mapper.Map<List<Skill>, List<SkillDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Skill.GetSkills, ""));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Skill.GetSkills, "", error));

            return Problem(error, Routes.Skill.GetSkills, 500);
        }
    }
}