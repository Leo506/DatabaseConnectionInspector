using System.Data;
using System.Data.Common;
using DbConnectionInspector.Connections;
using FluentAssertions;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class ConnectionCheckerTests
{
    [Fact]
    public async Task IsConnectionEstablish_Connection_Closed_Open_Connection()
    {
        // arrange
        var connection = new Mock<IDbConnection>();
        var wasInvoked = false;
        connection.SetupGet(dbConnection => dbConnection.State).Returns(ConnectionState.Closed);
        connection.Setup(dbConnection => dbConnection.Open()).Callback(() => wasInvoked = true);
        var sut = new ConnectionChecker(connection.Object);

        // act
        await sut.IsConnectionEstablish();

        // assert
        wasInvoked.Should().Be(true);
    }

    [Fact]
    public async Task IsConnectionEstablish_All_Good_Returns_True()
    {
        // arrange
        var command = new Mock<IDbCommand>();
        command.Setup(dbCommand => dbCommand.ExecuteScalar()).Returns("Success");
        var connection = new Mock<IDbConnection>();
        connection.Setup(dbConnection => dbConnection.CreateCommand()).Returns(command.Object);
        var sut = new ConnectionChecker(connection.Object);
        
        // act
        var result = await sut.IsConnectionEstablish();

        // assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task IsConnectionEstablish_Exception_While_Open_Connection_Returns_False()
    {
        // arrange
        var connection = new Mock<IDbConnection>();
        connection.Setup(dbConnection => dbConnection.Open()).Throws<Exception>();
        var sut = new ConnectionChecker(connection.Object);

        // act
        var result = await sut.IsConnectionEstablish();

        // assert
        result.Should().Be(false);
    }

    [Fact]
    public async Task IsConnectionEstablish_Exception_While_Execute_Command_Returns_False()
    {
        // arrange
        var command = new Mock<IDbCommand>();
        command.Setup(dbCommand => dbCommand.ExecuteScalar()).Throws<Exception>();
        var connection = new Mock<IDbConnection>();
        connection.Setup(dbConnection => dbConnection.CreateCommand()).Returns(command.Object);

        var sut = new ConnectionChecker(connection.Object);

        // act
        var result = await sut.IsConnectionEstablish();

        // assert
        result.Should().Be(false);
    }
}