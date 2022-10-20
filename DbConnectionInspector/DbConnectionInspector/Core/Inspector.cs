using System.Net;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DbConnectionInspector.Core;

/// <summary>
/// Class containing logic of connection checking
/// </summary>
public class Inspector
{
    private readonly RequestDelegate _next;
    private readonly Action<HttpContext?> _action;
    private readonly ILogger<Inspector>? _logger;
    private readonly ConnectionOptions _options;
    private readonly IEndpointMetadataExtractor _extractor;

    public Inspector(RequestDelegate next, ConnectionOptions options, IEndpointMetadataExtractor extractor,
        ILogger<Inspector>? logger = null)
    {
        _next = next;
        _options = options;
        _extractor = extractor;
        _action = context =>
        {
            if (context?.Response != null)
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        };
        _logger = logger;
    }

    public Inspector(RequestDelegate next, ConnectionOptions options, IEndpointMetadataExtractor extractor,
        ILogger<Inspector>? logger,
        Action<HttpContext?> action)
    {
        _next = next;
        _action = action;
        _extractor = extractor;
        _options = options;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var inspectionIsSuccess = true;
        var requirements = _extractor.Extract<RequireDbInspection>(context).ToList();
        
        if (!requirements.Any())
            _logger?.LogInformation(LoggingMessages.NoRequireInspection);

        foreach (var checker in requirements.SelectMany(requireDbInspection =>
                     FindAppropriateCheckers(requireDbInspection.ConnectionKey)))
        {
            if (await checker.IsConnectionEstablish()) continue;

            _logger?.LogError(string.Format(LoggingMessages.ConnectionFailed, checker));
            inspectionIsSuccess = false;
        }

        if (inspectionIsSuccess)
            await _next.Invoke(context);
        else
            _action.Invoke(context);
    }

    private IEnumerable<IConnectionChecker> FindAppropriateCheckers(string? key)
    {
        if (key is null)
            return _options.Checkers;
        
        var checker = _options.Checkers.FirstOrDefault(connectionChecker =>
            connectionChecker.Key == key);

        if (checker is null)
            return _options.Checkers;

        return new[] { checker };
    }
}