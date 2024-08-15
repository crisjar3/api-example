using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.Images;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Images;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.Images;

public class PostImage : EndpointBaseAsync.WithRequest<PostImageRequest>.WithActionResult<List<string>>
{
    private readonly IImageStorageService _avatarStorageService;
    private readonly ILogger<PostImage> _logger;
    private readonly UserManager<User> _userManager;

    public PostImage(IImageStorageService avatarStorageService, ILogger<PostImage> logger, UserManager<User> userManager)
    {
        _avatarStorageService = avatarStorageService;
        _logger = logger;
        _userManager = userManager;
    }

    [ProducesResponseType(201, Type = typeof(List<string>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpPost(Routes.Image.PostImage)]
    [SwaggerOperation(
    Summary = "Post an Image",
    Description = "Post an Image",
    OperationId = "Image.PostImage",
    Tags = new[] { "Image" })
    ]

    [Authorize]
    public override async Task<ActionResult<List<string>>> HandleAsync([FromForm] PostImageRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Image.PostImage, request, user.Email));

            var result = await _avatarStorageService.UploadImage(request.Images);

            if (result.IsSuccess)
            {
                _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Image.PostImage, request, user.Email));

                return Ok(result.Value);
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Image.PostImage, request, invalidError, user.Email));

                return Problem(invalidError, Routes.Image.PostImage, 400);
            }

            var error = ErrorHelper.GetErrors(result.Errors.ToList());

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Image.PostImage, request, error, user.Email));

            return Problem(error, Routes.Image.PostImage, 500);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Image.PostImage, request, error, user.Email));

            return Problem(error, Routes.Image.PostImage, 500);
        }
    }
}
