using System.Data;
using System.Data.Common;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

/// <summary>
/// Class for validating connection to database
/// </summary>
public class ConnectionChecker : IConnectionChecker, IDisposable
{
    public string? Key { get; set; }
    
    private readonly IDbConnection _connection;

    private static int _nextId = 1;
    private readonly int _instanceId;

    public ConnectionChecker(IDbConnection connection, string? key = null)
    {
        _connection = connection;

        _instanceId = _nextId;
        _nextId++;

        Key = key;
    }

    public async Task<bool> IsConnectionEstablish()
    {
        try
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteScalar();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }


    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    public override string ToString()
    {
        return
            $"{nameof(ConnectionChecker)}#{_instanceId}(connection: {_connection.GetType().Name}, " +
            $"connection string: \"{_connection.ConnectionString}\"";
    }
}