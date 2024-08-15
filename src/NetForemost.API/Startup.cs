using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetForemost.API.Configurations;
using NetForemost.API.Middlewares;
using NetForemost.Core.Interfaces.Images;
using NetForemost.Core.Services.Images;
using NetForemost.Infrastructure.Extensions;
using System;
using System.Linq;

namespace NetForemost.API;



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
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddGoogleServices();
        services.AddGoogleCloudBucket(Configuration);
        services.AddServices();
        services.AddScoped<IImageStorageService, ImageStorageService>();
        services.AddOptions(Configuration);
        services.AddRepositories();
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
        app.UseAuthorization();

        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseMiddleware<TokenManagerMiddleware>();
        app.UseMiddleware<LanguageMiddleware>();


        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}