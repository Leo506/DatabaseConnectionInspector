using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

public class ConnectionOptions
{
    public IDatabaseConnection[]? Connections { get; set; }
}