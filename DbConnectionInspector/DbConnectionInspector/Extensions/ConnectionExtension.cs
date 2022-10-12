using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace DbConnectionInspector.Extensions;

public static class ConnectionExtension
{
    public static IServiceCollection AddConnection<T>(this IServiceCollection collection, string connectionString)
        where T : class, IDatabaseConnection, new()
    {
        collection.AddSingleton<T>(new T()
        {
            ConnectionString = connectionString
        });
        return collection;
    }

    public static IServiceCollection AddConnection<T>(this IServiceCollection collection,
        Func<string> connStringBuilder)
        where T : class, IDatabaseConnection, new()
    {
        collection.AddSingleton<T>(new T()
        {
            ConnectionString = connStringBuilder.Invoke()
        });
        return collection;
    }
}