using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

/// <summary>
/// Data object with array of instances <see cref="IConnectionChecker"/>
/// </summary>
public class ConnectionOptions
{
    public IConnectionChecker[]? Checkers { get; set; }
}