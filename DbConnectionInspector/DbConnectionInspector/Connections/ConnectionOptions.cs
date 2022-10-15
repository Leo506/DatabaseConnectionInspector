using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

public class ConnectionOptions
{
    public IConnectionChecker[]? Connections { get; set; }
}