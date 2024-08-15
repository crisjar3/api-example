using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace NetForemost.API.Middlewares;

public static class MiddleWareConfiguration
{
    public static List<string> TokensLogout = new()
    {
        "POST:/api/v1/auth/login",
        "POST:/api/v1/auth/refresh-token",
        "POST:/api/v1/account/update-password",
        "POST:/api/v1/account/confirm-email",
        "POST:/api/v1/account/reset-password",
        "POST:/api/v1/account/reset-password-token"
    };

    public static List<string> Language = new()
    {
        "POST:/api/v1/auth/login"
    };

    public static bool ApplyFilter(List<string> endpoints, string currentEndpoint)
    {
        return endpoints.Any(endpoint => endpoint.Equals(currentEndpoint));
    }
    public static bool HasAuthorizationMiddleware(HttpContext context)
    {
        var endpointExist = context.GetEndpoint() is not null;

        if (!endpointExist)
        {
            return false;
        }

        var hasAuthorizeMiddleware = context.GetEndpoint()?.Metadata
            .Any(m => m.GetType() == typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));

        return (bool)hasAuthorizeMiddleware;
    }
}