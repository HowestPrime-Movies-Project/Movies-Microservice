using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class MovieEventTests
{
    [Fact]
    public void DefaultConstructor_ShouldLeavePropertiesUninitialized()
    {
        // Arrange & Act
        MovieEvent movieEvent = new();

        // Assert
        Assert.Equal(default, movieEvent.Id);
        Assert.Null(movieEvent.MovieId.Value == Guid.Empty ? null : movieEvent.MovieId);
        Assert.Null(movieEvent.RoomId.Value == Guid.Empty ? null : movieEvent.RoomId);
        Assert.Equal(default(DateTime), movieEvent.ShowTime);
        Assert.Equal(0, movieEvent.Capacity);
        Assert.Empty(movieEvent.Bookings);
        Assert.Equal(0, movieEvent.Visitors);
        Assert.Empty(movieEvent.DomainEvents);
    }

    [Fact]
    public void Create_WithValidState_ShouldRaiseDomainEvent()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);
        int capacity = 100;

        // Act
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, capacity);

        // Assert
        Assert.NotEqual(default, movieEvent.Id);
        Assert.Equal(movieId, movieEvent.MovieId);
        Assert.Equal(roomId, movieEvent.RoomId);
        Assert.Equal(futureShowTime, movieEvent.ShowTime);
        Assert.Equal(capacity, movieEvent.Capacity);
        Assert.Empty(movieEvent.Bookings);
        Assert.Equal(0, movieEvent.Visitors);
        Assert.Single(movieEvent.DomainEvents);

        var domainEvent = Assert.IsType<MovieEventScheduled>(movieEvent.DomainEvents.Single());
        Assert.Equal(movieEvent.Id, domainEvent.MovieEventId);
        Assert.Equal(movieId, domainEvent.MovieId);
        Assert.Equal(roomId, domainEvent.RoomId);
        Assert.Equal(futureShowTime, domainEvent.ShowTime);
    }

    [Fact]
    public void Create_WithShowtimeNotAt15hOr19h_ShouldThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(18); // 18h is not allowed

        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, futureShowTime, 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithShowtimeAt15h_ShouldSucceed()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);

        // Act
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, 100);

        // Assert
        Assert.Equal(futureShowTime, movieEvent.ShowTime);
    }

    [Fact]
    public void Create_WithShowtimeAt19h_ShouldSucceed()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(19);

        // Act
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, 100);

        // Assert
        Assert.Equal(futureShowTime, movieEvent.ShowTime);
    }

    [Fact]
    public void Create_WithShowtimeInPast_ShouldThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime pastShowTime = DateTime.Now.AddDays(-1).Date.AddHours(15);

        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, pastShowTime, 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithShowtimeNow_ShouldThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime nowShowTime = DateTime.Now;

        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, nowShowTime, 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithZeroCapacity_ShouldThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);

        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, futureShowTime, 0);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeCapacity_ShouldThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);

        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, futureShowTime, -50);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithLargeCapacity_ShouldSucceed()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(19);
        int largeCapacity = 10000;

        // Act
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, largeCapacity);

        // Assert
        Assert.Equal(largeCapacity, movieEvent.Capacity);
    }

    [Fact]
    public void ValidateState_WithValidState_ShouldNotThrow()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, 100);

        // Act
        Action act = () => movieEvent.ValidateState();

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void MovieEvent_Equality_SameReference_ShouldBeEqual()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());
        RoomId roomId = new(Guid.NewGuid());
        DateTime futureShowTime = DateTime.Now.AddDays(1).Date.AddHours(15);
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, futureShowTime, 100);

        // Act
        bool result = movieEvent.Equals(movieEvent);

        // Assert
        Assert.True(result);
    }
}


