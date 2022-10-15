using System.Data;
using System.Data.Common;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

public class ConnectionChecker : IConnectionChecker
{
    private readonly DbConnection _connection;

    public ConnectionChecker(DbConnection connection)
    {
        _connection = connection;
    }

    public Task<bool> IsConnectionEstablish()
    {
        return Task.FromResult(_connection.State == ConnectionState.Open);
    }
}