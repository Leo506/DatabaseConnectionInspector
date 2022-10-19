using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

/// <summary>
/// Data object with array of instances <see cref="IConnectionChecker"/>
/// </summary>
public class ConnectionOptions
{
    public IConnectionChecker[] Checkers { get; set; }

    public ConnectionOptions(params IConnectionChecker[] checkers)
    {
        var hash = new HashSet<string>(checkers.Where(c => c.Key != null).Select(c => c.Key)!);
        var actualCount = checkers.Count(c => c.Key is not null);
        if (hash.Count != actualCount)
            throw new InvalidOperationException("Can not add more than one IConnectionChecker with the same key");
        
        Checkers = checkers;
    }
}