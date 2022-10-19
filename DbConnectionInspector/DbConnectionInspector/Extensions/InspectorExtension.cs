using System.Data;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Extensions;

/// <summary>
/// Class containing extenstion methods for register Inspector middleware in DI
/// </summary>
public static class InspectorExtension
{
    /// <summary>
    /// Start using database connection inspection
    /// </summary>
    /// <param name="app">Your app builder</param>
    /// <param name="options">Data object with connection checkers</param>
    /// <returns><see cref="IApplicationBuilder"/> for use database connection inspector</returns>
    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app,
        ConnectionOptions options)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Inspector>>();
        return app.UseMiddleware<Inspector>(options, new EndpointMetadataExtractor(), logger);
    }

    /// <summary>
    /// Start using database connection inspection
    /// </summary>
    /// <param name="app">Your app builder</param>
    /// <param name="options">Data object with connection checkers</param>
    /// <returns><see cref="IApplicationBuilder"/> for use database connection inspector</returns>
    public static IApplicationBuilder UseDbConnectionInspector(this IApplicationBuilder app, ConnectionOptions? options,
        Action<HttpContext> action)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Inspector>>();
        return app.UseMiddleware<Inspector>(options, new EndpointMetadataExtractor(), logger, action);
    }
}