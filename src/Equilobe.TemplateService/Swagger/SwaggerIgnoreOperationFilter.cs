using Equilobe.TemplateService.Core.Common.Api;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Equilobe.TemplateService.Swagger;

public class SwaggerIgnoreOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null || context == null || context.ApiDescription?.ParameterDescriptions == null)
            return;

        var parametersToIgnore = context
            .ApiDescription
            .ParameterDescriptions
            .Where(parameterDescription => ParameterHasIgnoreAttribute(parameterDescription))
            .ToList();

        if (!parametersToIgnore.Any())
            return;

        foreach (var parameterToIgnore in parametersToIgnore)
        {
            var parameter = operation
                .Parameters
                .FirstOrDefault(parameter => string.Equals(parameter.Name, parameterToIgnore.Name, System.StringComparison.Ordinal));

            if (parameter == null)
                continue;

            operation.Parameters.Remove(parameter);
        }
    }

    private static bool ParameterHasIgnoreAttribute(ApiParameterDescription parameterDescription)
    {
        if (parameterDescription.ModelMetadata == null)
            return false;

        if (parameterDescription.ModelMetadata is not DefaultModelMetadata metadata)
            return false;

        if (metadata == null)
            return false;

        return metadata.Attributes.Attributes.Any(attribute => attribute.GetType() == typeof(SwaggerIgnoreAttribute));
    }
}