using DbConnectionInspector.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace DbConnectionInspector.Core;

public class EndpointMetadataExtractor : IEndpointMetadataExtractor
{
    public IEnumerable<T> Extract<T>(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        return endpoint is null ? new List<T>() : endpoint.Metadata.OfType<T>();
    }
}