using System.Net;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace DbConnectionInspector.UnitTests;

public partial class InspectorTests
{

    [Fact]
    public async Task InvokeAsync_No_Require_Inspection_Delegate_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor();
        var sut = new Inspector(delegateMock.Object, null, extractorMock.Object);

        // act
        await sut.InvokeAsync(default);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_No_Key_All_Good_Delegate_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor(new RequireDbInspection());

        var sut = new Inspector(delegateMock.Object, MakeOptions(new CheckerData(null, true)), extractorMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_No_Key_Failed_Default_Action_Invoke()
    {
        // arrange
        var extractorMock = MakeExtractor(new RequireDbInspection());

        var contextMock = new Mock<HttpContext>();
        contextMock.SetupSet(context => context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable)
            .Verifiable();

        var sut = new Inspector(new Mock<RequestDelegate>().Object,
            MakeOptions(new CheckerData(null, false)), extractorMock.Object);

        // act
        await sut.InvokeAsync(contextMock.Object);

        // assert
        contextMock.Verify();
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_One_Checkers_All_Good_Delegate_Invoke()
    {
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var sut = new Inspector(delegateMock.Object, MakeOptions(new CheckerData("Key1", true)), extractorMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_One_Checker_Failed_Default_Action_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var contextMock = new Mock<HttpContext>();
        contextMock.SetupSet(context => context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable)
            .Verifiable();

        var sut = new Inspector(delegateMock.Object, MakeOptions(new CheckerData("Key1", false)), extractorMock.Object);

        // act
        await sut.InvokeAsync(contextMock.Object);

        // assert
        contextMock.Verify();
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_Some_Checkers_All_Good_Delegate_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var sut = new Inspector(delegateMock.Object,
            MakeOptions(new CheckerData(null, true), new CheckerData(null, true)), extractorMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_Some_Checkers_Failed_Default_Action_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = MakeExtractor(new RequireDbInspection("Key1"));

        var contextMock = new Mock<HttpContext>();
        contextMock.SetupSet(context => context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable)
            .Verifiable();

        var sut = new Inspector(delegateMock.Object,
            MakeOptions(new CheckerData(null, false), new CheckerData(null, true)), extractorMock.Object);

        // act
        await sut.InvokeAsync(contextMock.Object);

        // assert
        contextMock.Verify();
    }


    private static Mock<IEndpointMetadataExtractor> MakeExtractor(params RequireDbInspection[] attributes)
    {
        var extractorMock = new Mock<IEndpointMetadataExtractor>();
        extractorMock.Setup(extractor => extractor.Extract<RequireDbInspection>(It.IsAny<HttpContext>()))
            .Returns(new List<RequireDbInspection>(attributes));
        return extractorMock;
    }

    private static ConnectionOptions MakeOptions(params CheckerData[] data)
    {
        var checkerList = new List<IConnectionChecker>();
        foreach (var checkerData in data)
        {
            var mock = new Mock<IConnectionChecker>();
            if (checkerData.Key != null)
                mock.SetupGet(m => m.Key).Returns(checkerData.Key);
            mock.Setup(m => m.IsConnectionEstablish()).ReturnsAsync(checkerData.ReturnValue);
            checkerList.Add(mock.Object);
        }

        return new ConnectionOptions(checkerList.ToArray());
    }
    
    private struct CheckerData
    {
        public readonly string? Key;
        public readonly bool ReturnValue;

        public CheckerData(string? key, bool returnValue)
        {
            Key = key;
            ReturnValue = returnValue;
        }
    }
}