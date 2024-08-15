using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.TalentPool;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.TalentsPool;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.TalentPool;
public class GetTalents : EndpointBaseAsync.WithRequest<GetTalentPoolRequest>.WithActionResult<PaginatedRecord<UserDto>>
{
    private readonly ILogger<GetTalents> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly ITalentPoolService _talentPoolService;

    public GetTalents(ILogger<GetTalents> logger, IMapper mapper, UserManager<User> userManager, ITalentPoolService talentPoolService)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _talentPoolService = talentPoolService;
    }


    [ProducesResponseType(200, Type = typeof(PaginatedRecord<UserDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Talent.GetTalents)]
    [SwaggerOperation(
        Summary = "Get all talents by many fields.",
        Description = "Get all talents by many fields.",
        OperationId = "Talent.GetTalents",
        Tags = new[] { "Talent" })
    ]
    [Authorize]
    public override async Task<ActionResult<PaginatedRecord<UserDto>>> HandleAsync([FromQuery] GetTalentPoolRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);

        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Talent.GetTalents, request, user.Email));

            var result = await _talentPoolService.FindTalentAsync(
                request.SkillsId,
                request.JobRolesId,
                request.SenioritiesId,
                request.Countries,
                request.Cities,
                request.Email,
                request.IsActive,
                request.StartRegistrationDate,
                request.EndRegistrationDate,
                request.Name,
                request.Languages,
                request.PageNumber,
                request.PerPage
                );

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<PaginatedRecord<User>, PaginatedRecord<UserDto>>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Talent.GetTalents, request, user.Email));
                return Ok(mapped);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Talent.GetTalents, request, error, user.Email));

            return Problem(error, Routes.Talent.GetTalents, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Talent.GetTalents, request, error, user.Email));

            return Problem(error, Routes.Talent.GetTalents, 500);
        }
    }
}
