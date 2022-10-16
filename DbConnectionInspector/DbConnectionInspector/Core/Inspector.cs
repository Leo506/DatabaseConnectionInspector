using System.Net;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Core;

public class Inspector
{
    private readonly RequestDelegate _next;
    private readonly Action<HttpContext?> _action;
    private readonly ILogger<Inspector>? _logger;

    public Inspector(RequestDelegate next, ILogger<Inspector>? logger = null)
    {
        _next = next;
        _action = context =>
        {
            if (context?.Response != null)
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        };
        _logger = logger;
    }

    public Inspector(RequestDelegate next, ILogger<Inspector>? logger, Action<HttpContext?> action)
    {
        _next = next;
        _action = action;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

        if (endpoint is null)
        {
            await _next.Invoke(context);
            return;
        }

        foreach (var require in endpoint.Metadata?.Where(metadata => metadata is IRequireDbInspection)
                     .Select(m => m as IRequireDbInspection)!)
        {
            if (!await require?.CreateConnectionChecker().IsConnectionEstablish()!)
            {
                _action.Invoke(context);
                return;
            }
        }

        await _next.Invoke(context);
    }
}