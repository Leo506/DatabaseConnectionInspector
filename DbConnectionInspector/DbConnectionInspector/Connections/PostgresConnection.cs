using System.Data;
using DbConnectionInspector.Abstractions;
using Npgsql;

namespace DbConnectionInspector.Connections;

public class PostgresConnection : IDatabaseConnection
{
    public string? ConnectionString { get; set; }

    public async Task<bool> IsConnectionOpen()
    {
        try
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            return conn.State == ConnectionState.Open;
        }
        catch (Exception)
        {
            return false;
        }
    }
}