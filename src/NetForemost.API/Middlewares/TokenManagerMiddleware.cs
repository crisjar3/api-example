using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Interfaces.Authentication;
using System.Net;
using System.Threading.Tasks;

namespace NetForemost.API.Middlewares;

public class TokenManagerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenManagerService _tokenManagerService;

    public TokenManagerMiddleware(RequestDelegate next, ITokenManagerService tokenManagerService)
    {
        _tokenManagerService = tokenManagerService;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!MiddleWareConfiguration.ApplyFilter(MiddleWareConfiguration.TokensLogout, context.Request.Path.ToString()))
        {
            if (await _tokenManagerService.IsCurrentActiveToken())
            {
                await _next.Invoke(context);

                return;
            }

            ProblemDetails error = new()
            {
                Title = "Unauthorized",
                Detail = "Unauthorized",
                Status = (int)HttpStatusCode.Unauthorized,
                Instance = context.Request.Path.ToString()
            };

            await context.Response.WriteAsJsonAsync(error);
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}