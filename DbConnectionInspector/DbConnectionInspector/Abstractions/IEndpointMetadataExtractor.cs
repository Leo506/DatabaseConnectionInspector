using Microsoft.AspNetCore.Http;

namespace DbConnectionInspector.Abstractions;

public interface IEndpointMetadataExtractor
{
    IEnumerable<T> Extract<T>(HttpContext context);
}