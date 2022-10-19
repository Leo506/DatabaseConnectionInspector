namespace DbConnectionInspector.Core;


/// <summary>
/// Attribute marks controller method as requiring a inspection connection to database 
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequireDbInspection : Attribute
{
    public string? ConnectionKey { get; private set; }

    public RequireDbInspection(string? key = null)
    {
        ConnectionKey = key;
    }
}