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
        // arrange
        var context = MakeMockContext(new Mock<RequestDelegate>().Object);

        var logger = new Mock<ILogger<Inspector>>();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, null, logger.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_No_Endpoint_Logger_Invoke()
    {
        // arrange
        var logger = new Mock<ILogger<Inspector>>(); ;

        var context = new Mock<HttpContext>();
        context.Setup(c => c.Features.Get<IEndpointFeature>()).Returns((IEndpointFeature)null);

        var sut = new Inspector(new Mock<RequestDelegate>().Object, null, logger.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Logger_Invoke()
    {
        // arrange
        var logger = new Mock<ILogger<Inspector>>();

        var checker = new Mock<IConnectionChecker>();
        checker.Setup(c => c.IsConnectionEstablish()).ReturnsAsync(false);

        var context = MakeMockContextWithMetadata(new Mock<RequestDelegate>().Object, new RequireDbInspection());

        var sut = new Inspector(new Mock<RequestDelegate>().Object,
            new ConnectionOptions() { Checkers = new[] { checker.Object } }, logger.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_No_Connection_Checkers_Logger_Invoke()
    {
        // arrange
        var logger = new Mock<ILogger<Inspector>>();

        var context = MakeMockContextWithMetadata(new Mock<RequestDelegate>().Object, new RequireDbInspection());

        var sut = new Inspector(new Mock<RequestDelegate>().Object, new ConnectionOptions(), logger.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        logger.Invocations.Count.Should().Be(1);
    }
}