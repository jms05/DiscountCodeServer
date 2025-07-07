using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JMS.Api.Extensions.IServiceCollectionExtensions.Swagger.SwaggerOptions;

[ExcludeFromCodeCoverage]
internal sealed class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
{
    public SwaggerConfigureOptions() { }

    public void Configure(SwaggerGenOptions c)
    {
        // Adding xml comments as doc
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
    }
}