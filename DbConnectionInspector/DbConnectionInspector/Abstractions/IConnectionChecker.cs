namespace DbConnectionInspector.Abstractions;

public interface IConnectionChecker
{
    Task<bool> IsConnectionEstablish();
    
    string? Key { get; set; }
}