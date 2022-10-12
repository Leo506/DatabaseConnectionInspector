using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Core;
using Microsoft.AspNetCore.Builder;

namespace DbConnectionInspector.Extensions;

public static class InspectorExtension
{
    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app,
        params IDatabaseConnection[] connections)
    {
        app.UseMiddleware<Inspector>(connections);
        return app;
    }
}