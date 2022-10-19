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
    public async Task InvokeAsync_Require_Inspection_No_Key_All_Checkers_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = new Mock<IEndpointMetadataExtractor>();
        extractorMock.Setup(extractor => extractor.Extract<RequireDbInspection>(It.IsAny<HttpContext>()))
            .Returns(new List<RequireDbInspection>() { new RequireDbInspection() });

        var actualInvokeCount = 0;

        var checkerOne = new Mock<IConnectionChecker>();
        checkerOne.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var checkerTwo = new Mock<IConnectionChecker>();
        checkerTwo.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var options = new ConnectionOptions(checkerOne.Object, checkerTwo.Object);
        var sut = new Inspector(delegateMock.Object, options, extractorMock.Object);

        // act
        await sut.InvokeAsync(default);

        // assert
        actualInvokeCount.Should().Be(2);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_One_Checker_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = new Mock<IEndpointMetadataExtractor>();
        extractorMock.Setup(extractor => extractor.Extract<RequireDbInspection>(It.IsAny<HttpContext>()))
            .Returns(new List<RequireDbInspection>() { new RequireDbInspection("key1") });

        var actualInvokeCount = 0;

        var checkerOne = new Mock<IConnectionChecker>();
        checkerOne.SetupGet(checker => checker.Key).Returns("key1");
        checkerOne.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var checkerTwo = new Mock<IConnectionChecker>();
        checkerTwo.SetupGet(checker => checker.Key).Returns("key2");
        checkerTwo.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var options = new ConnectionOptions(checkerOne.Object, checkerTwo.Object);
        var sut = new Inspector(delegateMock.Object, options, extractorMock.Object);

        // act
        await sut.InvokeAsync(default);

        // assert
        actualInvokeCount.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Require_Inspection_With_Key_All_Checkers_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var extractorMock = new Mock<IEndpointMetadataExtractor>();
        extractorMock.Setup(extractor => extractor.Extract<RequireDbInspection>(It.IsAny<HttpContext>()))
            .Returns(new List<RequireDbInspection>() { new RequireDbInspection("key1") });

        var actualInvokeCount = 0;

        var checkerOne = new Mock<IConnectionChecker>();
        checkerOne.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var checkerTwo = new Mock<IConnectionChecker>();
        checkerTwo.Setup(checker => checker.IsConnectionEstablish()).Callback(() => actualInvokeCount++);

        var options = new ConnectionOptions(checkerOne.Object, checkerTwo.Object);
        var sut = new Inspector(delegateMock.Object, options, extractorMock.Object);

        // act
        await sut.InvokeAsync(default!);

        // assert
        actualInvokeCount.Should().Be(2);
    }
}