using Microsoft.Extensions.Configuration;

namespace NetForemost.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static TProperties GetSettings<TProperties>(this IConfiguration configuration,
        string appSettingSectionName) where TProperties : new()
    {
        var properties = new TProperties();
        configuration.GetSection(appSettingSectionName).Bind(properties);
        return properties;
    }
}