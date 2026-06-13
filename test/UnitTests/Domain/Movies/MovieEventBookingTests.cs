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
        Assert.Single(movieEvent.Bookings);
        Assert.Equal(3, movieEvent.Visitors);
        Assert.Equal(2, booking.StandardVisitors);
        Assert.Equal(1, booking.DiscountVisitors);
        Assert.Equal(new[] { "2", "3", "4" }, booking.SeatNumbers);
    }

    [Fact]
    public void Book_TwoBookings_SeatsContinueSequentially()
    {
        DateTime showTime = DateTime.Now.AddDays(1).Date.AddHours(19);
        MovieEvent movieEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), showTime, 10);

        Booking first = movieEvent.Book(2, 0);
        Booking second = movieEvent.Book(1, 1);

        Assert.Equal(new[] { "2", "3" }, first.SeatNumbers);
        Assert.Equal(new[] { "4", "5" }, second.SeatNumbers);
        Assert.Equal(4, movieEvent.Visitors);
    }
}
