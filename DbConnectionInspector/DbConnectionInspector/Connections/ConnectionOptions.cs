using System.Collections;
using System.Data;
using DbConnectionInspector.Abstractions;

namespace DbConnectionInspector.Connections;

/// <summary>
/// Data object with array of instances <see cref="IConnectionChecker"/>
/// </summary>
public class ConnectionOptions : IEnumerable<IConnectionChecker>
{
    public List<IConnectionChecker> Checkers { get; set; }

    public ConnectionOptions(params IConnectionChecker[] checkers)
    {
        var hash = new HashSet<string>(checkers.Where(c => c.Key != null).Select(c => c.Key)!);
        var actualCount = checkers.Count(c => c.Key is not null);
        if (hash.Count != actualCount)
            throw new InvalidOperationException("Can not add more than one IConnectionChecker with the same key");
        
        Checkers = checkers.ToList();
    }

    public void Add(IConnectionChecker checker)
    {
        if (checker.Key != null && Checkers.Select(c => c.Key).Contains(checker.Key))
            throw new InvalidOperationException("Can not add more than one IConnectionChecker with the same key");
        
        Checkers.Add(checker);
    }

    public IEnumerator<IConnectionChecker> GetEnumerator()
    {
        return Checkers.ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}