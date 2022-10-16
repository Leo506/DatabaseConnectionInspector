using System.Data;
using System.Diagnostics;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;

namespace DbConnectionInspector.Core;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequireDbInspection : Attribute
{
    
}