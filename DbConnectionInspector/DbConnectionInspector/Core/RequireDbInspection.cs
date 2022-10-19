using System.Data;
using System.Diagnostics;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;

namespace DbConnectionInspector.Core;


/// <summary>
/// Attribute marks controller method as requiring a inspection connection to database 
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequireDbInspection : Attribute
{
    
}