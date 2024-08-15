using System.Security.Claims;

namespace NetForemost.API.Configurations.Extensions;

public static class ClaimsExtensions
{
    public static UserAttributes Attributes(this ClaimsPrincipal principal)
    {
        return new UserAttributes
            (principal.FindFirstValue(ClaimTypes.NameIdentifier),
            principal.FindFirstValue(ClaimTypes.Name),
            principal.FindFirstValue(ClaimTypes.Email)
            );

    }

    public record UserAttributes(string Id, string UserName, string Email);

}
