using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Account;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account;

public class GetUsersByRole : EndpointBaseAsync.WithRequest<GetUsersByRoleRequest>.WithActionResult<List<UserDto>>
{
    private readonly ILogger<GetUsersByRole> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetUsersByRole(ILogger<GetUsersByRole> logger, IMapper mapper, UserManager<User> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(List<UserDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(404, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.Account.GetUsersByRole)]
    [SwaggerOperation(
        Summary = "Get users by role",
        Description = "Get a list of all users under the specified role name.",
        OperationId = "Account.GetUsersByRole",
        Tags = new[] { "Account" }
    )]
    [Authorize]
    public override async Task<ActionResult<List<UserDto>>> HandleAsync([FromRoute] GetUsersByRoleRequest request, CancellationToken cancellationToken = default)
    {


        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.GetUsersByRole, request));

            var users = await _userManager.GetUsersInRoleAsync(request.RoleName);

            var mapped = _mapper.Map<List<User>, List<UserDto>>(users.ToList());

            return (mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetUsersByRole, request, error));

            return Problem(error, Routes.Account.GetUsersByRole, 500);
        }
    }
}