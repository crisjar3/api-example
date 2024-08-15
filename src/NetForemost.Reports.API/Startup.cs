using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NetForemost.Infrastructure.Extensions;
using NetForemost.Report.API.Configurations;
using NetForemost.Report.API.Middlewares;

namespace NetForemost.Report.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContexts(Configuration);
        services.AddGoogleServices();
        services.AddQueryClient(Configuration);
        services.AddRepositories();
        services.AddServices();
        services.AddOptions(Configuration);
        services.AddAuthentication(Configuration);
        services.AddCustomApiVersioning();
        services.AddProblemDetails();
        services.AddControllersWithViews();
        services.AddCors();
        services.AddLanguages();
        services.AddRedisCacheService(Configuration);
        services.AddControllers().AddJsonOptions(options => JsonExtension.ConfigureOptions(options.JsonSerializerOptions));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();

        app.UseSwaggerUI(config =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                config.SwaggerEndpoint(
                    url: $"/swagger/{description.GroupName}/swagger.json",
                    name: $"NetForemost API - {description.GroupName.ToUpper()}");
            }
        });

        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();
        app.UseProblemDetails();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseRequestLocalization();

        app.UseAuthentication();

        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseMiddleware<LanguageMiddleware>();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}