using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using ILogger = Castle.Core.Logging.ILogger;

namespace DbConnectionInspector.UnitTests;

public partial class InspectorTests
{

    [Fact]
    public async Task InvokeAsync_No_Require_Inspection_Logger_Invoke()
    {
        // arrange
        var extractorMock = MakeExtractor();

        var loggerMock = new Mock<ILogger<Inspector>>();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, MakeOptions(), extractorMock.Object,
            loggerMock.Object);
        
        // act
        await sut.InvokeAsync(default!);
        
        // assert
        loggerMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_No_Key_Failed_Logger_Invoke()
    {
        // arrange
        var extractorMock = MakeExtractor(new RequireDbInspection());

        var loggerMock = new Mock<ILogger<Inspector>>();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, MakeOptions(new CheckerData(null, false)),
            extractorMock.Object, loggerMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        loggerMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_Key_Not_Found_Failed_Logger_Invoke()
    {
        // arrange
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var loggerMock = new Mock<ILogger<Inspector>>();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, MakeOptions(new CheckerData(null, false)),
            extractorMock.Object, loggerMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        loggerMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_Failed_Logger_Invoke()
    {
        // arrange
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var loggerMock = new Mock<ILogger<Inspector>>();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, MakeOptions(new CheckerData("Key1", false)),
            extractorMock.Object, loggerMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        loggerMock.Invocations.Count.Should().Be(1);
    }
}