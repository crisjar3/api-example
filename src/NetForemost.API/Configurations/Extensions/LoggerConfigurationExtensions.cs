using Microsoft.Extensions.Configuration;
using NodaTime;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace NetForemost.API.Configurations.Extensions;

public static class LoggerConfigurationExtensions
{
    internal static LoggerConfiguration ConfigureBaseLogging(
        this LoggerConfiguration loggerConfiguration,
        string appName, IConfiguration configuration = null)
    {
        if (configuration is null)
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    true)
                .Build();

        loggerConfiguration
            .ReadFrom.Configuration(configuration, "Logging")
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)
            .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code))
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("ApplicationName", appName);

        return loggerConfiguration;
    }
}