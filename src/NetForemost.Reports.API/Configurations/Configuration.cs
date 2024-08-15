using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.CDN;
using NetForemost.Core.Entities.Emails;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Account;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Interfaces.Email;
using NetForemost.Core.Interfaces.Languages;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Core.Interfaces.Reports.GoalsReport;
using NetForemost.Core.Interfaces.Reports.TimeReports;
using NetForemost.Core.Interfaces.Reports.UserTimeLineReport;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.Core.Interfaces.TimeZonesReport;
using NetForemost.Core.Services.Account;
using NetForemost.Core.Services.Authentication;
using NetForemost.Core.Services.Email;
using NetForemost.Core.Services.Languages;
using NetForemost.Core.Services.Reports.GoalsReport;
using NetForemost.Core.Services.Reports.ProjectsReport;
using NetForemost.Core.Services.Reports.TimeReport;
using NetForemost.Core.Services.Reports.UserTimeLineReport;
using NetForemost.Core.Services.Settings;
using NetForemost.Core.Services.UserTimeLineReport;
using NetForemost.Infrastructure.Contexts;
using NetForemost.Infrastructure.Helpers;
using NetForemost.Infrastructure.Repositories;
using NetForemost.Report.API.Configurations.Swagger;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text;

#if RELEASE
using Npgsql;
#endif

namespace NetForemost.Report.API.Configurations;

public static class Configuration
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("development");

#if RELEASE
            // Equivalent connection string:
            // "Server=<dbSocketDir>/<INSTANCE_CONNECTION_NAME>;Uid=<DB_USER>;Pwd=<DB_PASS>;Database=<DB_NAME>"

            // Socket
            String dbSocketDir = Environment.GetEnvironmentVariable("DB_SOCKET_PATH") ?? "/cloudsql";


            // Postgres
            // Equivalent connection string:
            // "Server=<dbSocketDir>/<INSTANCE_CONNECTION_NAME>;Uid=<DB_USER>;Pwd=<DB_PASS>;Database=<DB_NAME>;Protocol=unix"
            String instancePostgresConnectionName = Environment.GetEnvironmentVariable("INSTANCE_POSTGRES_CONNECTION_NAME");
            var socketPostgresConnectionString = new NpgsqlConnectionStringBuilder()
            {
                // The Cloud SQL proxy provides encryption between the proxy and instance.
                SslMode = SslMode.Disable,
                // Remember - storing secrets in plain text is potentially unsafe. Consider using
                // something like https://cloud.google.com/secret-manager/docs/overview to help keep
                // secrets secret.
                Host = String.Format("{0}/{1}", dbSocketDir, instancePostgresConnectionName),
                Username = Environment.GetEnvironmentVariable("DB_POSTGRES_USER"), // e.g. 'my-db-user
                Password = Environment.GetEnvironmentVariable("DB_POSTGRES_PASS"), // e.g. 'my-db-password'
                Database = Environment.GetEnvironmentVariable("DB_POSTGRES_NAME"), // e.g. 'my-database'
            };
            socketPostgresConnectionString.Pooling = true;
            // Specify additional properties here.
            postgresConnectionString = socketPostgresConnectionString.ConnectionString;
#endif


        services.AddDbContext<NetForemostContext>(options =>
            options.UseNpgsql(postgresConnectionString,
                    x =>
                    {
                        x.EnableRetryOnFailure(12);
                    })
                .UseSnakeCaseNamingConvention()
                , ServiceLifetime.Transient
        );

        services.AddIdentity<User, Core.Entities.Roles.Role>()
            .AddEntityFrameworkStores<NetForemostContext>()
            .AddDefaultTokenProviders();

        return services;
    }
    public static IServiceCollection AddQueryClient(this IServiceCollection services, IConfiguration configuration)
    {

#if DEBUG
        string credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        var googleCredential = GoogleCredential.FromFile(credentialsPath);

        services.AddSingleton(provider =>
        {
            return BigQueryClient.Create("netforemost-dev", googleCredential);
        });
#endif

#if RELEASE
        string environment = Environment.GetEnvironmentVariable("ENV");
        services.AddSingleton(provider =>
        {
            return BigQueryClient.Create(environment);
        });

#endif
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICustomSpecification<>), typeof(CustomSpecification<>));
        services.AddScoped<IQueryBuilder, QueryBuilder>();
        services.AddTransient<IProjectAndGoalReportService, ProjectAndGoalReportService>();

        ////Core Services
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IUserSettingsService, UserSettingsService>();
        services.AddTransient<ITokenManagerService, TokenManagerService>();
        services.AddTransient<ILanguageService, LanguageService>();
        services.AddTransient<ISmtpClient, SmtpClientWrapper>();
        services.AddTransient<ITimeReportService, TimeReportService>();
        services.AddScoped<IUsersTimeLineReport, UsersTimeLinesReportService>();
        services.AddTransient<ITimeZoneReportService, TimeZoneReportService>();
        services.AddTransient<IGoalsReportService, GoalsReportService>();

        return services;
    }

    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // allows API return versions in the response header (api-supported-versions).
            config.ReportApiVersions = true;
        });

        //// Allows to discover versions
        services.AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(config =>
        {
            config.OperationFilter<SwaggerDefaultValuesFilter>();
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    public static IServiceCollection AddLanguages(this IServiceCollection services)
    {
        const string enUSCulture = "en";
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                    new CultureInfo(enUSCulture),
                    new CultureInfo("es")
                };
            options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
            {
                context.Request.Headers.TryGetValue(NameStrings.HeaderName_Language, out var value);
                return await Task.FromResult(new ProviderCultureResult(value.ToString()));
            }));
        });
        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication
        (options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Authentication:Jwt:Issuer"],
                ValidAudience = configuration["Authentication:Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:SecretKey"]))
            };
        });

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpClientConfig>(options => configuration.GetSection("Authentication:Email").Bind(options));
        services.Configure<JwtConfig>(options => configuration.GetSection("Authentication:Jwt").Bind(options));
        services.Configure<CloudStorageConfig>(options => configuration.GetSection("CloudStorage").Bind(options));
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBigQueryRepository<>), typeof(BigQueryRepository<>));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

        return services;
    }

    public static IServiceCollection AddRedisCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        string host = configuration["Authentication:Redis:Host"];
        string pass = configuration["Authentication:Redis:Password"];

        var multiplexer = ConnectionMultiplexer.Connect($"{host},password={pass}");
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        return services;
    }

    public static IServiceCollection AddGoogleServices(this IServiceCollection services)
    {
#if DEBUG
        string path = $"{Environment.CurrentDirectory}\\Credentials\\netforemost-dev-55ddf3b8ae10.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
#endif
        return services;
    }
}