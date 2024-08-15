using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using NetForemost.Infrastructure.Extensions;

namespace NetForemost.Infrastructure.Extensions;

public static class MigrationHelperExtension
{
    /// <summary>
    ///     Perform automatic migrations if isDev, otherwise simply EnsureCreated as migrations
    ///     should not be performed by the application in production environments.
    /// </summary>
    /// <param name="database">The database facade.</param>
    /// <param name="isDev">If the current environment is Development or not.</param>
    /// <param name="logger">A logger to log the actions taken to.</param>
    public static void Migrate(this DatabaseFacade database, bool isDev, ILogger logger)
    {
        if (isDev)
        {
            logger.LogInformation("In Development mode, performing automatic database migration.");
            database.Migrate();
        }
        else
        {
            logger.LogInformation(
                "In Production mode, no migrations performed. If migrations are required, run them manually.");
            database.EnsureCreated();
        }
    }
}