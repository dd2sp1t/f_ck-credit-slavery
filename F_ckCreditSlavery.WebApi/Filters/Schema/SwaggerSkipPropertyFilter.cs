using F_ckCreditSlavery.Entities.Attributes.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace F_ckCreditSlavery.WebApi.Filters.Schema;

public class SwaggerSkipPropertyFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var ignoredProperties = context.MethodInfo.GetParameters()
            .SelectMany(p => p.ParameterType.GetProperties()
                .Where(prop => prop.GetCustomAttribute<SwaggerIgnoreAttribute>() != null))
            .ToList();

        if (!ignoredProperties.Any()) return;

        foreach (var property in ignoredProperties)
        {
            operation.Parameters = operation.Parameters
                .Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture)))
                .ToList();
        }
    }
}