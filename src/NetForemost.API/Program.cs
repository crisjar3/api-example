using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NetForemost.API.Configurations.Extensions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace NetForemost.API;

public class Program
{
    private const string AppName = "NetForemost API";

    public static async Task Main(string[] args)
    {
        String dbSocketDir = Environment.GetEnvironmentVariable("DB_SOCKET_PATH") ?? "/cloudsql";

        Log.Logger = new LoggerConfiguration()
            .ConfigureBaseLogging(AppName)
            .CreateLogger();

        try
        {
            Log.Information("Starting web host");
            var host = CreateHostBuilder(args).Build();

            host.MigrateDatabase();
            await host.SeedDatabaseAsync();
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}