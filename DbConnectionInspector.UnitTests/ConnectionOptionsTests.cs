using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using FluentAssertions;
using Moq;

namespace DbConnectionInspector.UnitTests;

public class ConnectionOptionsTests
{
    [Fact]
    public void CreateOptions_Checkers_Without_Key_Success()
    {
        // arrange
        var sut = new ConnectionOptions(new Mock<IConnectionChecker>().Object, new Mock<IConnectionChecker>().Object);
        
        // assert
        sut.Checkers.Count.Should().Be(2);
    }

    [Fact]
    public void CreateOptions_Checkers_With_Duplicate_Key_Throws()
    {
        // arrange
        var checkerOne = new Mock<IConnectionChecker>();
        checkerOne.SetupGet(c => c.Key).Returns("key");

        var checkerTwo = new Mock<IConnectionChecker>();
        checkerTwo.SetupGet(c => c.Key).Returns("key");

        // act and assert
        Assert.Throws<InvalidOperationException>(() => new ConnectionOptions(checkerOne.Object, checkerTwo.Object));
    }

    [Fact]
    public void CreateOptions_Checkers_With_Unique_Key_Success()
    {
        // arrange
        var checkerOne = new Mock<IConnectionChecker>();
        checkerOne.SetupGet(c => c.Key).Returns("key1");

        var checkerTwo = new Mock<IConnectionChecker>();
        checkerTwo.SetupGet(c => c.Key).Returns("key2");

        // act
        var sut = new ConnectionOptions(checkerOne.Object, checkerTwo.Object);

        // assert
        sut.Checkers.Count.Should().Be(2);
    }

    [Fact]
    public void CreateOptions_No_Checkers_Success()
    {
        // act
        var sut = new ConnectionOptions();

        // assert
        sut.Checkers.Count.Should().Be(0);
    }
}