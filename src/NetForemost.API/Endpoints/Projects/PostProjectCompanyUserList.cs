using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
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
    public class PostProjectCompanyUserList : EndpointBaseAsync.WithRequest<PostProjectCompanyUserListRequest>.WithActionResult<List<ProjectCompanyUserStatusDto>>
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<PostProjectCompanyUserList> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PostProjectCompanyUserList(IProjectService projectService, ILogger<PostProjectCompanyUserList> logger, IMapper mapper, UserManager<User> userManager)
        {
            _projectService = projectService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [ProducesResponseType(201, Type = typeof(List<ProjectCompanyUserStatusDto>))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [HttpPost(Routes.Project.PostProjectCompanyUserList)]
        [SwaggerOperation(
            Summary = "Assign projects to company User.",
            Description = "Add a list of projects to the company User.",
            OperationId = "Project.PostProjectCompanyUserList",
            Tags = new[] { "Project" })
        ]
        [Authorize]
        public override async Task<ActionResult<List<ProjectCompanyUserStatusDto>>> HandleAsync(PostProjectCompanyUserListRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Project.PostProjectCompanyUserList, request, user.Email));

                var result = await _projectService.AddProjectCompanyUsersList(request.CompanyUserId, request.ProjectListIds, user.Id);

                if (result.IsSuccess)
                {
                    var mapped = _mapper.Map<List<ProjectCompanyUser>, List<ProjectCompanyUserStatusDto>>(result.Value);

                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Project.PostProjectCompanyUserList, request, user.Email));

                    return Ok(mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);
                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserList, request, invalidError, user.Email));

                    return Problem(invalidError, Routes.Project.PostProjectCompanyUserList, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserList, request, error, user.Email));

                return Problem(error, Routes.Project.PostProjectCompanyUserList, 500);
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);
                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Project.PostProjectCompanyUserList, request, error, user.Email));

                return Problem(error, Routes.Project.PostProjectCompanyUserList, 500);
            }
        }
    }
}

