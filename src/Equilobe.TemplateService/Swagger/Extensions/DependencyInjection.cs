using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace Equilobe.TemplateService.Swagger.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(swg =>
        {
            swg.EnableAnnotations();
            swg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Enter the JWT in the following format 'Bearer AUTH_CODE'",
                Scheme = "Bearer"
            });
            swg.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });

            swg.SchemaFilter<SwaggerIgnoreSchemaFilter>();
            swg.OperationFilter<SwaggerIgnoreOperationFilter>();

            swg.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }
                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return new[] { controllerActionDescriptor.ControllerName };
                }
                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });

            swg.DocInclusionPredicate((name, api) => true);
        });
    }
}

