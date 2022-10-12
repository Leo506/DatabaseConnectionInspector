using System.Data;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using Microsoft.AspNetCore.Builder;

namespace DbConnectionInspector.Extensions;

public static class InspectorExtension
{
    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app,
        ConnectionOptions options)
    {
        return app.UseMiddleware<Inspector>(options);
    }
}