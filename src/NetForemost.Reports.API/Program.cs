using NetForemost.Report.API.Configurations.Extensions;
using Serilog;

namespace NetForemost.Report.API;

public class Program
{
    private const string AppName = "NetForemost Report API";

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ConfigureBaseLogging(AppName)
            .CreateLogger();

        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}