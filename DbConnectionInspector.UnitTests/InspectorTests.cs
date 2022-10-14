using Castle.Core.Logging;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class InspectorTests
{
    [Fact]
    public async Task InvokeAsync_Connection_Established_Success()
    {
        // arrange
        var connection = new Mock<IDatabaseConnection>();
        connection.Setup(conn => conn.IsConnectionOpen()).Returns(Task.FromResult(true));
        var requestDelegate = new Mock<RequestDelegate>();
        var sut = new Inspector(requestDelegate.Object, new ConnectionOptions()
        {
            Connections = new[] { connection.Object }
        }, null);

        // act
        await sut.InvokeAsync(default);

        // assert
        requestDelegate.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Write_503_Code()
    {
        // arrange
        var connection = new Mock<IDatabaseConnection>();
        connection.Setup(conn => conn.IsConnectionOpen()).Returns(Task.FromResult(false));
        var sut = new Inspector(new Mock<RequestDelegate>().Object, new ConnectionOptions()
        {
            Connections = new[] { connection.Object }
        }, null);
        var context = new Mock<HttpContext>();
        context.SetupSet(httpContext => httpContext.Response.StatusCode = 503).Verifiable();
        
        // act
        await sut.InvokeAsync(context.Object);

        // assert
        context.Verify();
    }

    [Fact]
    public async Task InvokeAsync_No_Connections_Success()
    {
        // arrange
        var requestDelegate = new Mock<RequestDelegate>();
        var sut = new Inspector(requestDelegate.Object, null, null);

        // act
        await sut.InvokeAsync(default);

        // assert
        requestDelegate.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Specify_Action_Action_Invoked()
    {
        // arrange
        var connection = new Mock<IDatabaseConnection>();
        connection.Setup(conn => conn.IsConnectionOpen()).Returns(Task.FromResult<bool>(false));
        var sut = new Inspector(new Mock<RequestDelegate>().Object, new ConnectionOptions()
            {
                Connections = new[] { connection.Object }
            },
            null, (HttpContext context) => context.Response.StatusCode = 404);
        var context = new Mock<HttpContext>();
        context.SetupSet(httpContext => httpContext.Response.StatusCode = 404).Verifiable();

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        context.Verify();
    }

    [Fact]
    public async Task InvokeAsync_No_Connections_Logger_Invoked()
    {
        // arrange
        var logger = new Mock<ILogger<Inspector>>();
        var sut = new Inspector(new Mock<RequestDelegate>().Object, null, logger.Object);

        // act
        await sut.InvokeAsync(default);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Logger_Invoked()
    {
        // arrange
        var connection = new Mock<IDatabaseConnection>();
        connection.Setup(conn => conn.IsConnectionOpen()).Returns(Task.FromResult<bool>(false));
        var logger = new Mock<ILogger<Inspector>>();
        var sut = new Inspector(new Mock<RequestDelegate>().Object, new ConnectionOptions()
        {
            Connections = new[] { connection.Object }
        }, logger.Object);

        // act
        await sut.InvokeAsync(default);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }
}