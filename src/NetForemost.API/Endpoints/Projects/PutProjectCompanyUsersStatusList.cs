using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Projects;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Projects
{
    public class PutProjectCompanyUsersStatusList : EndpointBaseAsync.WithRequest<PutProjectCompanyUserStatusListRequest>.WithActionResult<List<ProjectCompanyUserStatusDto>>
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<PutProjectCompanyUsersStatusList> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PutProjectCompanyUsersStatusList(IProjectService projectService, ILogger<PutProjectCompanyUsersStatusList> logger, IMapper mapper, UserManager<User> userManager)
        {
            _projectService = projectService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(201, Type = typeof(List<ProjectCompanyUserStatusDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPut(Routes.Project.PutProjectCompanyUsersActiveStatusList)]
        [SwaggerOperation(
            Summary = "Update the status of user in a list of project.",
            Description = "Update the status of a companuy user into a list of project.",
            OperationId = "Project.PutProjectCompanyUsersActiveStatusList",
            Tags = new[] { "Project" })
        ]
        [Authorize]
        public override async Task<ActionResult<List<ProjectCompanyUserStatusDto>>> HandleAsync(PutProjectCompanyUserStatusListRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PutProjectCompanyUsersActiveStatusList, request, user.Email));

                var result = await _projectService.UpdateProjectCompanyUsersListActiveStatus(request.CompanyUserId, request.ProjectListIds, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<ProjectCompanyUser>, List<ProjectCompanyUserStatusDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PutProjectCompanyUsersActiveStatusList, request, user.Email));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUsersActiveStatusList, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Project.PutProjectCompanyUsersActiveStatusList, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUsersActiveStatusList, request, error, user.Email));

                return Problem(error, Routes.Project.PutProjectCompanyUsersActiveStatusList, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);
                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PutProjectCompanyUsersActiveStatusList, request, error, user.Email));

                return Problem(error, Routes.Project.PutProjectCompanyUsersActiveStatusList, 500);
            }
        }
    }
}
