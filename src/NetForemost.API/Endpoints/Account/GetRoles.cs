using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Interfaces.Account;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Account
{
    public class GetRoles : EndpointBaseAsync.WithoutRequest.WithActionResult<List<RoleDto>>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<GetRoles> _logger;
        private readonly IMapper _mapper;

        public GetRoles(IMapper mapper, ILogger<GetRoles> logger,
            IAccountService accountService)
        {
            _mapper = mapper;
            _logger = logger;
            _accountService = accountService;
        }

        [ProducesResponseType(200, Type = typeof(RoleDto))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.Account.GetRoles)]
        [SwaggerOperation(
            Summary = "Get all existing roles for a user.",
            Description = "Get all the roles that a user can have.",
            OperationId = "Account.GetRoles",
            Tags = new[] { "Account" })
        ]
        [Authorize]
        public override async Task<ActionResult<List<RoleDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var user = User.Attributes();
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.GetRoles, "", user.Email));

                var result = await _accountService.GetRolesAsync();

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<Role>, List<RoleDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.GetRoles, "", user.Email));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = string.Join(",", result.ValidationErrors.Select(x => x.ErrorMessage));

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.GetRoles, "", invalidError, user.Email));

                    return Problem(invalidError, Routes.Account.GetRoles, 400);
                }

                var error = string.Join(",", result.Errors);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetRoles, "", error, user.Email));

                return Problem(error, Routes.Account.GetRoles, 500);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.GetRoles, "", error, user.Email));

                return Problem(error, Routes.Account.GetRoles, 500);
            }
        }
    }
}