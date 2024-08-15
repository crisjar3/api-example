using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Enumerations.Language;
using NetForemost.Core.Interfaces.Languages;
using NetForemost.SharedKernel.Properties;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NetForemost.API.Middlewares;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;
    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (MiddleWareConfiguration.HasAuthorizationMiddleware(context))
        {
            using (var scope = context.RequestServices.CreateScope())
            {

                //Use Service
                var languageService = scope.ServiceProvider.GetRequiredService<ILanguageService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<LanguageMiddleware>>();

                //Get the language
                var ClaimLanguageCode = context.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimType.LanguageCode);
                var ClaimLanguageId = context.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaimType.Language);
                var languageId = int.Parse(ClaimLanguageId.Value);
                var languageCode = ClaimLanguageCode.Value;

                //other information
                var path = context.Request.Path.ToString();
                var method = context.Request.Method.ToString();

                context.Request.Headers.Set(NameStrings.HeaderName_Language, languageId.ToString());
                CultureInfo.CurrentCulture = new CultureInfo(languageCode);
                CultureInfo.CurrentUICulture = new CultureInfo(languageCode);
                logger.LogInformation($"The request of the path {path} by the method {method} to validate the language:{languageCode} has been successful.");
                await _next.Invoke(context);
                return;
            }
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}