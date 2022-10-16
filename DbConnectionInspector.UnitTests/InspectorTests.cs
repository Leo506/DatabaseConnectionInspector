using System.Data;
using System.Net;
using Castle.Core.Logging;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class InspectorTests
{
    [Fact]
    public async Task InvokeAsync_Extract_Endpoint_Success()
    {
        // arrange
        
        var success = false;
        
        var delegateMock = new Mock<RequestDelegate>();
        
        var context = MakeMockContext(delegateMock.Object);
        context.Setup(c => c.Features.Get<IEndpointFeature>()).Callback(() => success = true);
        
        var sut = new Inspector(delegateMock.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        success.Should().Be(true);
    }

    [Fact]
    public async Task InvokeAsync_Extract_Metadata_Success()
    {
        // arrange
        var attributeMock = new Mock<IRequireDbInspection>();
        attributeMock.Setup(a => a.CreateConnectionChecker()).Returns(new Mock<IConnectionChecker>().Object);
        
        var context = MakeMockContextWithMetadata(new Mock<RequestDelegate>().Object, attributeMock.Object);
        
        var sut = new Inspector(new Mock<RequestDelegate>().Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        attributeMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_No_Require_Inspection_Delegate_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        var context = MakeMockContext(delegateMock.Object);
        var sut = new Inspector(delegateMock.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_All_Good_Delegate_Invoke()
    {
        // arrange
        var delegateMock = new Mock<RequestDelegate>();
        
        var checkerMock = new Mock<IConnectionChecker>();
        checkerMock.Setup(c => c.IsConnectionEstablish()).Returns(Task.FromResult<bool>(true));
        
        var attributeMock = new Mock<IRequireDbInspection>();
        attributeMock.Setup(atr => atr.CreateConnectionChecker()).Returns(checkerMock.Object);
        
        var context = MakeMockContextWithMetadata(delegateMock.Object, attributeMock.Object);

        var sut = new Inspector(delegateMock.Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        delegateMock.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Default_Action_Invoke()
    {
        // arrange
        var checkerMock = new Mock<IConnectionChecker>();
        checkerMock.Setup(c => c.IsConnectionEstablish()).ReturnsAsync(false);

        var attributeMock = new Mock<IRequireDbInspection>();
        attributeMock.Setup(atr => atr.CreateConnectionChecker()).Returns(checkerMock.Object);

        var context = MakeMockContextWithMetadata(new Mock<RequestDelegate>().Object, attributeMock.Object);
        context.SetupSet(c => c.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable).Verifiable();
        
        var sut = new Inspector(new Mock<RequestDelegate>().Object);

        // act
        await sut.InvokeAsync(context.Object);

        // assert
        context.Verify();
    }

    [Fact]
    public async Task InvokeAsync_Connection_Failed_Specify_Action_Invoke()
    {
        // arrange
        var checkerMock = new Mock<IConnectionChecker>();
        checkerMock.Setup(c => c.IsConnectionEstablish()).ReturnsAsync(false);

        var attributeMock = new Mock<IRequireDbInspection>();
        attributeMock.Setup(atr => atr.CreateConnectionChecker()).Returns(checkerMock.Object);

        var context = MakeMockContextWithMetadata(new Mock<RequestDelegate>().Object, attributeMock.Object);
        context.SetupSet(c => c.Response.StatusCode = (int)HttpStatusCode.BadRequest).Verifiable();

        var sut = new Inspector(new Mock<RequestDelegate>().Object, null, httpContext =>
        {
            if (httpContext != null) httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        });
        
        // act
        await sut.InvokeAsync(context.Object);

        // assert
        context.Verify();
    }

    private Mock<HttpContext> MakeMockContext(RequestDelegate @delegate)
    {
        var endpoint = new Endpoint(@delegate, EndpointMetadataCollection.Empty, "");
        var featureMock = new Mock<IEndpointFeature>();
        featureMock.SetupGet(feature => feature.Endpoint).Returns(endpoint);
        var context = new Mock<HttpContext>();
        context.Setup(httpContext => httpContext.Features.Get<IEndpointFeature>()).Returns(featureMock.Object);

        return context;
    }

    private Mock<HttpContext> MakeMockContextWithMetadata(RequestDelegate @delegate, params object[] metadata)
    {
        var endpoint = new Endpoint(@delegate, new EndpointMetadataCollection(metadata), "");
        var featureMock = new Mock<IEndpointFeature>();
        featureMock.SetupGet(feature => feature.Endpoint).Returns(endpoint);
        var context = new Mock<HttpContext>();
        context.Setup(httpContext => httpContext.Features.Get<IEndpointFeature>()).Returns(featureMock.Object);

        return context;
    }
}