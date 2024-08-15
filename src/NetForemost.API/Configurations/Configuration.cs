using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetForemost.API.Configurations;
using NetForemost.API.Configurations.Swagger;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.CDN;
using NetForemost.Core.Entities.Emails;
using NetForemost.Core.Entities.SendGrid;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Account;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Interfaces.Benefits;
using NetForemost.Core.Interfaces.Cities;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Interfaces.ContractTypes;
using NetForemost.Core.Interfaces.Countries;
using NetForemost.Core.Interfaces.Dashboards;
using NetForemost.Core.Interfaces.Email;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Interfaces.Industries;
using NetForemost.Core.Interfaces.JobOffers;
using NetForemost.Core.Interfaces.JobRoles;
using NetForemost.Core.Interfaces.Languages;
using NetForemost.Core.Interfaces.Policies;
using NetForemost.Core.Interfaces.PriorityLevels;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.Core.Interfaces.SendGrid;
using NetForemost.Core.Interfaces.Seniorities;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.Core.Interfaces.Skills;
using NetForemost.Core.Interfaces.StoryPoints;
using NetForemost.Core.Interfaces.TalentsPool;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.Core.Interfaces.Timer;
using NetForemost.Core.Services.Account;
using NetForemost.Core.Services.Authentication;
using NetForemost.Core.Services.Benefits;
using NetForemost.Core.Services.Cities;
using NetForemost.Core.Services.Companies;
using NetForemost.Core.Services.ContractTypes;
using NetForemost.Core.Services.Countries;
using NetForemost.Core.Services.Dashboards;
using NetForemost.Core.Services.Email;
using NetForemost.Core.Services.Goals;
using NetForemost.Core.Services.Industries;
using NetForemost.Core.Services.JobOffers;
using NetForemost.Core.Services.JobRoles;
using NetForemost.Core.Services.Languages;
using NetForemost.Core.Services.Policies;
using NetForemost.Core.Services.PriorityLevels;
using NetForemost.Core.Services.Projects;
using NetForemost.Core.Services.SendGrid;
using NetForemost.Core.Services.Seniorities;
using NetForemost.Core.Services.Settings;
using NetForemost.Core.Services.Skills;
using NetForemost.Core.Services.StoryPoints;
using NetForemost.Core.Services.TalentsPool;
using NetForemost.Core.Services.Tasks;
using NetForemost.Core.Services.TimeTrackings;
using NetForemost.Infrastructure.Contexts;
using NetForemost.Infrastructure.Importer;
using NetForemost.Infrastructure.Repositories;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

#if RELEASE
using Npgsql;
#endif

namespace NetForemost.API.Configurations;

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
                    x => { x.MigrationsAssembly("NetForemost.Infrastructure.Migrations"); })
                .UseSnakeCaseNamingConvention(),
                ServiceLifetime.Transient
        );

        services.AddIdentity<User, Core.Entities.Roles.Role>()
            .AddEntityFrameworkStores<NetForemostContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //Core Services
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<ICompanySettingsService, CompanySettingsService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IUserSettingsService, UserSettingsService>();
        services.AddTransient<ITokenManagerService, TokenManagerService>();
        services.AddTransient<IJobRoleService, JobRoleService>();
        services.AddTransient<IBenefitService, BenefitService>();
        services.AddTransient<ISkillService, SkillService>();
        services.AddTransient<ICountryService, CountryService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IDashboardService, DashboardService>();
        services.AddTransient<ISeniorityService, SeniorityService>();
        services.AddTransient<IIndustryService, IndustryService>();
        services.AddTransient<IGoalService, GoalService>();
        services.AddTransient<ILanguageService, LanguageService>();
        services.AddTransient<IContractTypeService, ContractTypeService>();
        services.AddTransient<IPolicyService, PolicyService>();
        services.AddTransient<ITalentPoolService, TalentPoolService>();
        services.AddTransient<IPriorityLevelService, PriorityLevelService>();
        services.AddTransient<IStoryPointService, StoryPointService>();
        services.AddTransient<ITaskTypeService, TaskTypeService>();
        services.AddTransient<ITaskService, TaskService>();
        services.AddTransient<ICompanyUserService, CompanyUserService>();
        services.AddTransient<IJobOfferService, JobOfferService>();
        services.AddTransient<IGoalStatusService, GoalStatusService>();
        services.AddTransient<ITimerService, TimerService>();
        services.AddTransient<ICompanyUserInvitationService, CompanyUserInvitationService>();
        services.AddTransient<ISendGridService, SendGridService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ISmtpClient, SmtpClientWrapper>();

        //Importer
        services.AddTransient<Importer>();

        services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

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

        // Allows to discover versions
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
        services.Configure<SendGridApiConfig>(options => configuration.GetSection("Authentication:SendGrid").Bind(options));
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

    public static IServiceCollection AddGoogleCloudBucket(this IServiceCollection services, IConfiguration configuration)
    {
        var storageClient = StorageClient.Create();

#if DEBUG
        string credentialsPath = $"{Environment.CurrentDirectory}\\Credentials\\netforemost-dev-55ddf3b8ae10.json";
        storageClient = StorageClient.Create(GoogleCredential.FromFile(credentialsPath));
#endif
        services.AddSingleton(storageClient);
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