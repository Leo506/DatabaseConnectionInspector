using DbConnectionInspector.Connections;

namespace DbConnectionInspector.IntegrationTests;

public class PostgreConnectionTests
{
    [Fact]
    public async Task IsConnectionOpen_Connection_Open_Returns_True()
    {
        // arrange
        var sut = new PostgresConnection("User ID=admin;Password=password;Host=localhost;Port=5432;Database=Test");

        // act
        var result = await sut.IsConnectionOpen();

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsConnectionOpen_Incorrect_Connection_String_Returns_False()
    {
        // arrange
        var sut = new PostgresConnection("User ID=admin;Password=password;Host=localhost;Port=5433;Database=Test");

        // act
        var result = await sut.IsConnectionOpen();

        // assert
        Assert.False(result);
    }
}