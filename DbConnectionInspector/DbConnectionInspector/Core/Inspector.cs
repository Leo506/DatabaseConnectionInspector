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
    private readonly ConnectionOptions _options;

    public Inspector(RequestDelegate next, ConnectionOptions options, ILogger<Inspector>? logger = null)
    {
        _next = next;
        _options = options;
        _action = context =>
        {
            if (context?.Response != null)
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        };
        _logger = logger;
    }

    public Inspector(RequestDelegate next, ConnectionOptions options, ILogger<Inspector>? logger, Action<HttpContext?> action)
    {
        _next = next;
        _action = action;
        _options = options;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

        if (endpoint is null)
        {
            _logger?.LogInformation(StringConstants.NoEndpoint);
            await _next.Invoke(context);
            return;
        }

        if (_options?.Checkers == null)
        {
            _logger?.LogInformation(StringConstants.NoConnectionsProvided);
            await _next.Invoke(context);
            return;
        }

        if (endpoint.Metadata.Any(m => m is RequireDbInspection))
        {
            foreach (var connectionChecker in _options.Checkers)
            {
                if (!await connectionChecker.IsConnectionEstablish())
                {
                    _logger?.LogInformation(string.Format(StringConstants.ConnectionFailed,
                        connectionChecker.ToString()));
                    _action.Invoke(context);
                    return;
                }
            }
        }

        _logger?.LogInformation(StringConstants.NoRequireInspection);
        await _next.Invoke(context);
    }
}