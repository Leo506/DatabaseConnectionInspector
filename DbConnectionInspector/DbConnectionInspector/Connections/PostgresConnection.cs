using System.Data;
using DbConnectionInspector.Abstractions;
using Npgsql;

namespace DbConnectionInspector.Connections;

public class PostgresConnection : IDatabaseConnection
{
    private readonly string _connectionString;

    public PostgresConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> IsConnectionOpen()
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn.State == ConnectionState.Open;
        }
        catch (Exception)
        {
            return false;
        }
    }
}