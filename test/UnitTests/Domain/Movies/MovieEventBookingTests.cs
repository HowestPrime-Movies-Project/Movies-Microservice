using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;
using System;
using System.Linq;

namespace UnitTests.Domain.Movies;

public class MovieEventBookingTests
{
    [Fact]
    public void Book_WithZeroTotalVisitors_ThrowsInvalidOperationException()
    {
        // Arrange
        MovieEvent movieEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), DateTime.Now.AddDays(1).Date.AddHours(19), 100);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => movieEvent.Book(0, 0));
        Assert.Equal("The total number of visitors must be greater than 0.", ex.Message);
    }

    [Fact]
    public void Book_ExceedingCapacity_ThrowsInvalidOperationException()
    {
        // Arrange
        MovieEvent movieEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), DateTime.Now.AddDays(1).Date.AddHours(19), 2);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => movieEvent.Book(2, 1));
        Assert.Equal("Cannot book more visitors than the capacity of the movie event.", ex.Message);
    }

    [Fact]
    public void Book_ScheduledMoreThan14DaysInAdvance_ThrowsInvalidOperationException()
    {
        // Arrange: schedule more than 14 days in the future (15 days) at an allowed hour
        DateTime farFuture = DateTime.Now.AddDays(15).Date.AddHours(19);
        MovieEvent movieEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), farFuture, 100);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => movieEvent.Book(1, 0));
        Assert.Equal("Cannot book movie events scheduled more than 14 days in advance.", ex.Message);
    }

    [Fact]
    public void Book_ValidInput_AddsBookingAndRaisesEvent()
    {
        // Arrange
        DateTime showTime = DateTime.Now.AddDays(1).Date.AddHours(19);
        MovieEvent movieEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), showTime, 10);

        // Act
        Booking booking = movieEvent.Book(2, 1);

        // Assert
        Assert.NotNull(booking);
        Assert.Equal(1, movieEvent.Bookings.Count);
        Assert.Equal(3, movieEvent.Visitors);
        Assert.Equal(2, booking.StandardVisitors);
        Assert.Equal(1, booking.DiscountVisitors);
        // Booking.Create currently doesn't retain provided seat numbers, so expect empty list
        Assert.Empty(booking.SeatNumbers);
    }
}
