using System.Net;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using Microsoft.AspNetCore.Http;

namespace DbConnectionInspector.Core;

public class Inspector
{
    private readonly RequestDelegate _next;
    private readonly ConnectionOptions? _connectionOptions;
    private Action<HttpContext> _action;

    public Inspector(RequestDelegate next, ConnectionOptions connectionOptions)
    {
        _next = next;
        _connectionOptions = connectionOptions;
        _action = context => context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
    }

    public Inspector(RequestDelegate next, ConnectionOptions connectionOptions, Action<HttpContext> action)
    {
        _next = next;
        _connectionOptions = connectionOptions;
        _action = action;
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
                _action.Invoke(context);
                return;
            }
        }

        await _next.Invoke(context);
    }
}