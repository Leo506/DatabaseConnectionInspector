using System.Data;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Extensions;

public static class InspectorExtension
{
    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app,
        ConnectionOptions options)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Inspector>>();
        return app.UseMiddleware<Inspector>(options, logger);
    }

    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app, ConnectionOptions? options,
        Action<HttpContext> action)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Inspector>>();
        return app.UseMiddleware<Inspector>(options, logger, action);
    }
}