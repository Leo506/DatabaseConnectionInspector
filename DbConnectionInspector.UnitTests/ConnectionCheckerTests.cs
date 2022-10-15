using System.Data;
using System.Data.Common;
using DbConnectionInspector.Connections;
using FluentAssertions;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class ConnectionCheckerTests
{
    [Theory]
    [InlineData(ConnectionState.Open, true)]
    [InlineData(ConnectionState.Closed, false)]
    public async Task IsConnectionEstablish_Check_State_Success(ConnectionState state, bool expected)
    {
        // arrange
        var connection = new Mock<DbConnection>();
        connection.SetupGet(dbConnection => dbConnection.State).Returns(state);
        var sut = new ConnectionChecker(connection.Object);

        // act
        var result = await sut.IsConnectionEstablish();
        
        // assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task IsConnectionEstablish_Should_First_Open_Connection_Success()
    {
        // arrange
        var connection = new Mock<DbConnection>();
        connection.SetupGet(dbConnection => dbConnection.State).Returns(ConnectionState.Closed);
        connection.Setup(dbConnection => dbConnection.Open()).Callback(() =>
            connection.SetupGet(dbConnection => dbConnection.State).Returns(ConnectionState.Open));

        var sut = new ConnectionChecker(connection.Object);
        
        // act
        var result = await sut.IsConnectionEstablish();

        // assert
        result.Should().Be(true);
    }
}