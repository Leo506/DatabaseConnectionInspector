namespace DbConnectionInspector.Abstractions;

public interface IDatabaseConnection
{
    Task<bool> IsConnectionOpen();
}