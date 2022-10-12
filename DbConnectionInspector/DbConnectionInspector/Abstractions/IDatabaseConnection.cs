namespace DbConnectionInspector.Abstractions;

public interface IDatabaseConnection
{
    public string ConnectionString { get; set; }
    Task<bool> IsConnectionOpen();
}