using System.Data;
using System.Data.Common;
using DbConnectionInspector.Connections;
using FluentAssertions;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class ConnectionCheckerTests
{
    [Fact]
    public async Task IsConnectionEstablish_All_Good_Returns_True()
    {
        // arrange
        var connection = new Mock<DbConnection>();
        connection.SetupGet(dbConnection => dbConnection.State).Returns(ConnectionState.Open);
        var sut = new ConnectionChecker(connection.Object);

        // act
        var result = await sut.IsConnectionEstablish();
        
        // assert
        result.Should().Be(true);
    }
}