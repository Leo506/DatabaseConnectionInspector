using DbConnectionInspector.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace DbConnectionInspector.Extensions;

public static class ConnectionExtension
{
    public static IServiceCollection AddPostgresConnection(this IServiceCollection collection, string connectionString)
    {
        collection.AddSingleton(new PostgresConnection(connectionString));
        return collection;
    }

    public static IServiceCollection AddPostgresConnection(this IServiceCollection collection,
        Func<string> connStringBuilder)
    {
        collection.AddSingleton(new PostgresConnection(connStringBuilder.Invoke()));
        return collection;
    }
}