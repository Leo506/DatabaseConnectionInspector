using DbConnectionInspector.Core;
using DbConnectionInspector.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class EndpointMetadataExtractorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Extract_Some_Requirement_Success(int count)
    {
        // arrange
        var requirementCollection = new List<RequireDbInspection>();
        for (var i = 0; i < count; i++)
        {
            requirementCollection.Add(new RequireDbInspection());
        }

        var endpoint = new Endpoint(new Mock<RequestDelegate>().Object,
            new EndpointMetadataCollection(requirementCollection), "");

        var context = MakeContextWithEndpoint(endpoint);

        var sut = new EndpointMetadataExtractor();

        // act
        var actual = sut.Extract<RequireDbInspection>(context);

        // assert
        actual.Count().Should().Be(count);
    }

    [Fact]
    public void Extract_No_Requirement_Returns_Empty()
    {
        // arrange
        var endpoint = new Endpoint(new Mock<RequestDelegate>().Object, EndpointMetadataCollection.Empty, "");

        var context = MakeContextWithEndpoint(endpoint);

        var sut = new EndpointMetadataExtractor();
        
        // act
        var actual = sut.Extract<RequireDbInspection>(context);

        // assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Extract_Endpoint_Feature_Is_Null_Returns_Empty()
    {
        // arrange

        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Features.Get<IEndpointFeature>()).Returns(null as IEndpointFeature);

        var sut = new EndpointMetadataExtractor();

        // act
        var actual = sut.Extract<RequireDbInspection>(contextMock.Object);

        // assert
        actual.Should().BeEmpty();
    }

    private HttpContext MakeContextWithEndpoint(Endpoint endpoint)
    {
        var featuresMock = new Mock<IEndpointFeature>();
        featuresMock.SetupGet(feature => feature.Endpoint).Returns(endpoint);
        
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(c => c.Features.Get<IEndpointFeature>()).Returns(featuresMock.Object);

        return contextMock.Object;
    }
}