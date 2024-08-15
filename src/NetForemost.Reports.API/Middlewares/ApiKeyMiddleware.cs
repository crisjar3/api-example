using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Report.API.Middlewares;
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        using (var scope = context.RequestServices.CreateScope())
        {
            // Use services
            var authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApiKeyMiddleware>>();

            //Get the ApiKey
            var apiKey = context.Request.Headers[NameStrings.HeaderName_ApiKey];

            //other information
            var path = context.Request.Path.ToString();
            var method = context.Request.Method.ToString();

            logger.LogInformation($"A request has been made to the following path {path} the following method {method} to validate the apiKey :{apiKey}");

            if (string.IsNullOrEmpty(apiKey))
            {
                logger.LogInformation($"The request of the path {path} by the method {method} to validate the apikey:{apiKey} has been invalid because the apikey is empty");

                ProblemDetails missingApiKeyError = new()
                {
                    Title = "Unauthorized",
                    Detail = "Missing ApiKey",
                    Status = 401,
                    Instance = context.Request.Path.ToString()
                };

                await context.Response.WriteAsJsonAsync(missingApiKeyError);
                return;
            }

            var result = await authenticationService.VerifyApiKey(apiKey);

            if (result.IsSuccess)
            {
                logger.LogInformation($"The request of the path {path} by the method {method} to validate the apikey:{apiKey} has been successful.");
                await _next.Invoke(context);
                return;
            }

            if (result.Status == ResultStatus.Invalid)
            {
                var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());
                logger.LogError($"The request of the path {path} by the method {method} to validate the apikey:{apiKey} has been invalid for the following reason {invalidError}");
            }
            else
            {
                var error = ErrorHelper.GetErrors(result.Errors.ToList());
                logger.LogError($"The request of the path {path} by the method {method} to validate the apikey:{apiKey} has occurred a validation error for the following reason {error}");
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            ProblemDetails invalidApiKey = new()
            {
                Title = "Unauthorized",
                Detail = "Invalid ApiKey",
                Status = 401,
                Instance = context.Request.Path.ToString()
            };

            await context.Response.WriteAsJsonAsync(invalidApiKey);
            return;
        }
    }
}
