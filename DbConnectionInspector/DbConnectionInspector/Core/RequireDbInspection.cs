using System.Data;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;

namespace DbConnectionInspector.Core;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequireDbInspection : Attribute, IRequireDbInspection
{
    private readonly IDbConnection _connection;

    public RequireDbInspection(IDbConnection connection)
    {
        _connection = connection;
    }

    public IConnectionChecker CreateConnectionChecker()
    {
        return new ConnectionChecker(_connection);
    }
}