using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Authentication;
using NetForemost.Core.Dtos.AppClients;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Authentication;

public class PostAppClient : EndpointBaseAsync.WithRequest<PostAppClientRequest>.WithActionResult<AppClientDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<PostAppClient> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public PostAppClient(IAuthenticationService authenticationService, ILogger<PostAppClient> logger, IMapper mapper, UserManager<User> userManager)
    {
        _authenticationService = authenticationService;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [ProducesResponseType(200, Type = typeof(AppClientDto))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Authentication.PostAppClient)]
    [SwaggerOperation(
        Summary = "Post a new app client.",
        Description = "Generate a new api key for an app.",
        OperationId = "Authentication.PostAppClient",
        Tags = new[] { "Authentication" })
    ]
    [Authorize]
    public override async Task<ActionResult<AppClientDto>> HandleAsync(PostAppClientRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Authentication.PostAppClient, request, user.Email));

            var appClient = _mapper.Map<PostAppClientRequest, AppClient>(request);
            var result = await _authenticationService.CreateAppClientAsync(appClient, user.Id);

            if (result.IsSuccess)
            {
                var mapped = _mapper.Map<AppClient, AppClientDto>(result.Value);

                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Authentication.PostAppClient, request, user.Email));

                return Created($"Created {nameof(AppClient)}", mapped);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Authentication.PostAppClient, request, error, user.Email));

            return Problem(error, Routes.Authentication.PostAppClient, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Authentication.PostAppClient, request, error, user.Email));

            return Problem(error, Routes.Authentication.PostAppClient, 500);
        }
    }
}
