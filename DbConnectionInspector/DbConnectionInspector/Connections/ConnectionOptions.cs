using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

public class ConnectionOptions
{
    public IConnectionChecker[]? Checkers { get; set; }
}