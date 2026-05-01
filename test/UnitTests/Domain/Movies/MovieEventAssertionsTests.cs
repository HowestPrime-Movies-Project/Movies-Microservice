using Howestprime.Movies.Domain.Movies;

namespace UnitTests.Domain.Movies;

public sealed class MovieEventAssertionsTests
{
    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt15h_ShouldNotThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 15, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt19h_ShouldNotThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 19, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt14h_ShouldThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 14, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt18h_ShouldThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 18, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt20h_ShouldThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 20, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt0h_ShouldThrow()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 0, 0, 0);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithFutureShowtime_ShouldNotThrow()
    {
        // Arrange
        DateTime futureShowTime = DateTime.Now.AddDays(1);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsInTheFuture(futureShowTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithPastShowtime_ShouldThrow()
    {
        // Arrange
        DateTime pastShowTime = DateTime.Now.AddDays(-1);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsInTheFuture(pastShowTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithCurrentTime_ShouldThrow()
    {
        // Arrange
        DateTime currentTime = DateTime.Now;

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsInTheFuture(currentTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithFarFutureShowtime_ShouldNotThrow()
    {
        // Arrange
        DateTime farFutureShowTime = DateTime.Now.AddYears(5);

        // Act
        Action act = () => MovieEventAssertions.EnsureShowtimeIsInTheFuture(farFutureShowTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowtimeIsAt15hOr19h_WithShowtimeAt15hHasExceptionMessage()
    {
        // Arrange
        DateTime showTime = new DateTime(2026, 5, 10, 12, 0, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(showTime));
        Assert.Contains("15h or 19h", exception.Message);
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithPastTimeHasExceptionMessage()
    {
        // Arrange
        DateTime pastShowTime = DateTime.Now.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => MovieEventAssertions.EnsureShowtimeIsInTheFuture(pastShowTime));
        Assert.Contains("future", exception.Message);
    }
}

