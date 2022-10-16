using System.Data;

namespace DbConnectionInspector.Abstractions;

public interface IRequireDbInspection
{
    IConnectionChecker CreateConnectionChecker();
}