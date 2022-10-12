using DbConnectionInspector.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class ConnectionExtensionTests
{
    [Fact]
    public void AddPostgresConnection_Via_Connection_String_Success()
    {
        // arrange
        var collection = new Mock<IServiceCollection>();

        // act
        ConnectionExtension.AddPostgresConnection(collection.Object, "connection string");


        // assert
        collection.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public void AddPostgresConnection_Via_Lambda_Success()
    {
        // arrange
        var collection = new Mock<IServiceCollection>();

        // act
        ConnectionExtension.AddPostgresConnection(collection.Object, () => "connection string");

        // assert
        collection.Invocations.Count.Should().Be(1);
    }
}