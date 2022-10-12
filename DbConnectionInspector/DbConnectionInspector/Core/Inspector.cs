using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using Microsoft.AspNetCore.Http;

namespace DbConnectionInspector.Core;

public class Inspector
{
    private readonly RequestDelegate _next;
    private readonly ConnectionOptions? _connectionOptions;

    public Inspector(RequestDelegate next, ConnectionOptions connectionOptions)
    {
        _next = next;
        _connectionOptions = connectionOptions;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_connectionOptions == null)
        {
            await _next.Invoke(context);
            return;
        }
        
        foreach (var databaseConnection in _connectionOptions.Connections)
        {
            if (!await databaseConnection.IsConnectionOpen())
            {
                context.Response.StatusCode = 503;
                return;
            }
        }

        await _next.Invoke(context);
    }
}