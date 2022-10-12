using DbConnectionInspector.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DbConnectionInspector.Core;

public class Inspector
{
    private readonly RequestDelegate _delegate;
    private readonly IDatabaseConnection[] _connections;

    public Inspector(RequestDelegate @delegate, params IDatabaseConnection[] connections)
    {
        _delegate = @delegate;
        _connections = connections;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var databaseConnection in _connections)
        {
            if (await databaseConnection.IsConnectionOpen()) continue;
            
            context.Response.StatusCode = 503;
            return;
        }

        await _delegate(context);
    }
}