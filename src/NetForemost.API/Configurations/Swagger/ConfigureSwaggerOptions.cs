using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetForemost.API.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetForemost.API.Configurations.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                                    Enter 'Bearer' [space] and then your token in the text input below. 
                                    Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

        options.OperationFilter<ApiKeyFilter>();
        options.OperationFilter<LanguageFilter>();

        options.EnableAnnotations();

        foreach (var description in this.apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            options.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(this.DefaultSchemaIdSelector);
        }
    }

    private string DefaultSchemaIdSelector(Type modelType)
    {
        if (!modelType.IsConstructedGenericType) return modelType.Name;

        var prefix = modelType.GetGenericArguments()
            .Select(this.DefaultSchemaIdSelector)
            .Aggregate((previous, current) => previous + current);

        return modelType.Name.Split('`').First() + "Of" + prefix;
    }

    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Version = "v1.0.0Alpha",
            Title = "NetForemost",
            Description = "NetForemost API",
            Contact = new OpenApiContact
            {
                Name = "NetForemost",
                Email = "martin@netforemost.com",
                Url = new Uri("https://www.netforemost.com/"),
            },
            License = new OpenApiLicense
            {
                Name = "Use for NetForemost products",
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " (deprecated)";
        }

        return info;
    }
}
