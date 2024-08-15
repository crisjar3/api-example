using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Configurations.Extensions;
using NetForemost.API.Requests.JobRoles;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Interfaces.JobRoles;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.JobRoles
{
    public class GetJobRoles : EndpointBaseAsync.WithRequest<GetJobRolesRequest>.WithActionResult<List<JobRoleDetailsDto>>
    {
        private readonly IJobRoleService _jobRoleService;
        private readonly ILogger<GetJobRoles> _logger;
        private readonly IMapper _mapper;

        public GetJobRoles(IMapper mapper, ILogger<GetJobRoles> logger,
            IJobRoleService jobRoleService)
        {
            _mapper = mapper;
            _logger = logger;
            _jobRoleService = jobRoleService;
        }

        [ProducesResponseType(200, Type = typeof(List<JobRoleDetailsDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpGet(Routes.JobRole.GetJobRoles)]
        [SwaggerOperation(
            Summary = "Get all job roles.",
            Description = "Get all job that belong to a company.",
            OperationId = "JobRole.GetJobRoles",
            Tags = new[] { "JobRole" })
        ]
        public override async Task<ActionResult<List<JobRoleDetailsDto>>> HandleAsync([FromQuery] GetJobRolesRequest request, CancellationToken cancellationToken = default)
        {
            var user = User.Attributes();
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.JobRole.GetJobRoles, request));

                var result = await _jobRoleService.GetJobRolesAsync(request.CompanyId, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<JobRole>, List<JobRoleDetailsDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.JobRole.GetJobRoles, request));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoles, request, invalidError));

                    return Problem(invalidError, Routes.JobRole.GetJobRoles, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoles, request, error));

                return Problem(error, Routes.JobRole.GetJobRoles, 500);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.JobRole.GetJobRoles, "", error, user.Email));

                return Problem(error, Routes.JobRole.GetJobRoles, 500);
            }
        }
    }
}
