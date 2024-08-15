using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetForemost.SharedKernel.Properties;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;using AutoMapper;

namespace NetForemost.API.Filters
{
    public class LanguageFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var currentEndpoint = context.ApiDescription.RelativePath;
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = NameStrings.HeaderName_Language,
                In = ParameterLocation.Header,
                Required = false,
                Description = NameStrings.HeaderName_Language,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("")
                }
            });
        }
    }
}


public class Origin
{
    private string name
    {
        get;
        set;
    }

public class destiny
{
    string name { get; set; }
}

public static class Mapsper{

    public T map<Torigin, TDestiny>()
}