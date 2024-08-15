using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Policies;
using NetForemost.Core.Dtos.Policies;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Policies;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Policies;

public class GetPolicies : EndpointBaseAsync.WithRequest<GetPolicyRequest>.WithActionResult<List<PolicyDto>>
{
    private readonly IPolicyService _policyService;
    private readonly ILogger<GetPolicies> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetPolicies(UserManager<User> userManager, IMapper mapper, ILogger<GetPolicies> logger, IPolicyService policyService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _policyService = policyService;
    }

    [ProducesResponseType(200, Type = typeof(List<PolicyDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Policy.GetPolicies)]
    [SwaggerOperation(
        Summary = "Get all policies.",
        Description = "Get all policies",
        OperationId = "Policy.GetPolicies",
        Tags = new[] { "Policy" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<PolicyDto>>> HandleAsync([FromQuery] GetPolicyRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Policy.GetPolicies, request, user.Email));

            var result = await _policyService.FindPoliciesAsync(request.CompanyId);

            var mapped = _mapper.Map<List<Policy>, List<PolicyDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Policy.GetPolicies, request, user.Email));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Policy.GetPolicies, request, error, user.Email));

            return Problem(error, Routes.Policy.GetPolicies, 500);
        }
    }
}