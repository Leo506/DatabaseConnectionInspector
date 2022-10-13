using System.Data;
using DbConnectionInspector.Abstractions;
using Npgsql;

namespace DbConnectionInspector.Connections;

public class PostgresConnection : IDatabaseConnection, IDisposable
{
    public string? ConnectionString { get; set; }
    private NpgsqlConnection? _connection = null;
    private bool _isOpen = false;

    public async Task<bool> IsConnectionOpen()
    {
        try
        {

            if (_connection == null)
            {
                _connection = new NpgsqlConnection(ConnectionString);
                _connection.Settings.KeepAlive = 1;
                _connection.StateChange += (sender, args) => _isOpen = _connection.FullState == ConnectionState.Open;

                await _connection.OpenAsync();

                _isOpen = _connection.FullState == ConnectionState.Open;
            }

            if (_isOpen == false)
            {
                await _connection.OpenAsync();
                _isOpen = _connection.FullState == ConnectionState.Open;
            }

            return _isOpen;

        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}