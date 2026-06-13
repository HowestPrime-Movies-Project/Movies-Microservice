using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public class MovieEventCloseBookingTests
{
    private static MovieEvent CreateMovieEvent(int capacity = 10)
    {
        return MovieEvent.Create(
            new MovieId(Guid.NewGuid()),
            new RoomId(Guid.NewGuid()),
            DateTime.UtcNow.AddDays(1).Date.Add(new TimeSpan(19, 0, 0)),
            capacity
        );
    }

    [Fact]
    public void CloseBooking_BookingNotFound_ThrowsInvalidOperationException()
    {
        var movieEvent = CreateMovieEvent();
        var nonExistentBookingId = new BookingId(Guid.NewGuid());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            movieEvent.CloseBooking(nonExistentBookingId, CloseBookingReason.PaymentSuccess));

        Assert.Contains(nonExistentBookingId.Value.ToString(), exception.Message);
    }

    [Fact]
    public void CloseBooking_PaymentSuccess_ClosesBookingWithoutReleasingVisitors()
    {
        var movieEvent = CreateMovieEvent();
        var booking = movieEvent.Book(2, 1);
        int visitorsBeforeClose = movieEvent.Visitors;

        movieEvent.CloseBooking(booking.Id, CloseBookingReason.PaymentSuccess);

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Success, booking.PaymentStatus);
        Assert.Equal(visitorsBeforeClose, movieEvent.Visitors);
    }

    [Fact]
    public void CloseBooking_PaymentFailed_ClosesBookingAndReleasesVisitors()
    {
        var movieEvent = CreateMovieEvent();
        var booking = movieEvent.Book(2, 1);

        movieEvent.CloseBooking(booking.Id, CloseBookingReason.PaymentFailed);

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Failed, booking.PaymentStatus);
        Assert.Equal(0, movieEvent.Visitors);
    }

    [Fact]
    public void CloseBooking_PaymentFailed_EmptiesSeatsOnBooking()
    {
        var movieEvent = CreateMovieEvent();
        var booking = movieEvent.Book(2, 1);

        movieEvent.CloseBooking(booking.Id, CloseBookingReason.PaymentFailed);

        Assert.Empty(booking.SeatNumbers);
        Assert.Equal(0, booking.StandardVisitors);
        Assert.Equal(0, booking.DiscountVisitors);
    }
}
