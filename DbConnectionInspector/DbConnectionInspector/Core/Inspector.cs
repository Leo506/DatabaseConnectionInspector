using System.Net;
using DbConnectionInspector.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Core;

public class Inspector
{
    private readonly RequestDelegate _next;
    private readonly ConnectionOptions? _connectionOptions;
    private readonly Action<HttpContext?> _action;
    private readonly ILogger<Inspector>? _logger;

    public Inspector(RequestDelegate next, ConnectionOptions? connectionOptions, ILogger<Inspector>? logger)
    {
        _next = next;
        _connectionOptions = connectionOptions;
        _action = context =>
        {
            if (context != null)
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        };
        _logger = logger;
    }

    public Inspector(RequestDelegate next, ConnectionOptions? connectionOptions, ILogger<Inspector>? logger, Action<HttpContext?> action)
    {
        _next = next;
        _connectionOptions = connectionOptions;
        _action = action;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_connectionOptions == null)
        {
            _logger?.LogWarning(StringConstants.NoConnectionsProvided);
            await _next.Invoke(context);
            return;
        }

        if (_connectionOptions.Connections == null)
        {
            await _next.Invoke(context);
            return;
        }
        
        foreach (var databaseConnection in _connectionOptions.Connections)
        {
            if (!await databaseConnection.IsConnectionOpen())
            {
                _logger?.LogError(string.Format(StringConstants.ConnectionFailed, databaseConnection.GetType().Name));
                _action.Invoke(context);
                return;
            }
        }

        await _next.Invoke(context);
    }
}