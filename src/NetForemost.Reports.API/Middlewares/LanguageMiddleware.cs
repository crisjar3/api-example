namespace NetForemost.Report.API.Middlewares;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;
    private const string LanguageCodeDefault = "en";
    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        using (var scope = context.RequestServices.CreateScope())
        {
            await _next.Invoke(context);
            return;

            ////Use Service
            //var languageService = scope.ServiceProvider.GetRequiredService<ILanguageService>();
            //var logger = scope.ServiceProvider.GetRequiredService<ILogger<LanguageMiddleware>>();

            ////Get the language
            //var language = context.Request.Headers[NameStrings.HeaderName_Language];

            ////other information
            //var path = context.Request.Path.ToString();
            //var method = context.Request.Method.ToString();

            //logger.LogInformation($"A request has been made to the following path {path} the following method {method} to validate the existence of: {language}");

            //if (string.IsNullOrEmpty(language))
            //{
            //    language = LanguageCodeDefault;
            //    context.Request.Headers[NameStrings.HeaderName_Language] = LanguageCodeDefault;
            //}

            //var result = await languageService.ValidateLanguageAsync(language);

            //if (result.IsSuccess)
            //{
            //    context.Request.Headers.Set(NameStrings.HeaderName_Language, result.Value.ToString());
            //    logger.LogInformation($"The request of the path {path} by the method {method} to validate the language:{language} has been successful.");
            //    await _next.Invoke(context);
            //    return;
            //}

            //if (result.Status == ResultStatus.Invalid)
            //{
            //    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList());
            //    logger.LogError($"The request of the path {path} by the method {method} to validate the Language:{language} has been invalid for the following reason {invalidError}");
            //}
            //else
            //{
            //    var error = ErrorHelper.GetErrors(result.Errors.ToList());
            //    logger.LogError($"The request of the path {path} by the method {method} to validate the Language:{language} has occurred a validation error for the following reason {error}");
            //}

            //context.Response.StatusCode = StatusCodes.Status400BadRequest;

            //ProblemDetails invalidLanguage = new()
            //{
            //    Title = "Bad Request",
            //    Detail = "The Language Specified is not right",
            //    Status = 400,
            //    Instance = context.Request.Path.ToString()
            //};

            //await context.Response.WriteAsJsonAsync(invalidLanguage);
            //return;
        }
    }
}