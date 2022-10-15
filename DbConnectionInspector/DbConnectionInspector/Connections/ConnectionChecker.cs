using System.Data;
using System.Data.Common;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

public class ConnectionChecker : IConnectionChecker, IDisposable
{
    private readonly DbConnection _connection = null!;
    private bool _isEstablish;

    public ConnectionChecker(DbConnection connection)
    {
        try
        {
            _connection = connection;
            _connection.StateChange += (sender, args) => _isEstablish = _connection.State == ConnectionState.Open;
            _connection.Open();
            _isEstablish = _connection.State == ConnectionState.Open;
        }
        catch
        {
            _isEstablish = false;
        }
    }

    public Task<bool> IsConnectionEstablish()
    {
        return Task.FromResult(_isEstablish);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}