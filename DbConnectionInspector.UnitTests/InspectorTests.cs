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

    private Mock<HttpContext> MakeMockContext(RequestDelegate @delegate)
    {

        var endpoint = new Endpoint(@delegate, EndpointMetadataCollection.Empty, "");
        var featureMock = new Mock<IEndpointFeature>();
        featureMock.SetupGet(feature => feature.Endpoint).Returns(endpoint);
        var context = new Mock<HttpContext>();
        context.Setup(httpContext => httpContext.Features.Get<IEndpointFeature>()).Returns(featureMock.Object);

        return context;
    }
}