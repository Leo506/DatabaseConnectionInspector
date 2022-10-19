using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;

namespace DbConnectionInspector.UnitTests;

public partial class InspectorTests
{

    [Fact]
    public async Task InvokeAsync_No_Require_Inspection_Logger_Invoke()
    {
        
    }

    [Fact]
    public async Task InvokeAsync_No_Endpoint_Logger_Invoke()
    {
        
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Logger_Invoke()
    {
        
    }

    [Fact]
    public async Task InvokeAsync_No_Connection_Checkers_Logger_Invoke()
    {
        
    }
}