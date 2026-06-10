using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public class BookingCloseTests
{
    [Fact]
    public void Close_PaymentSuccess_SetsClosedStatusAndSuccessPayment()
    {
        var booking = Booking.Create(2, 1, new List<string>());

        booking.Close(CloseBookingReason.PaymentSuccess);

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Succes, booking.PaymentStatus);
    }

    [Fact]
    public void Close_PaymentSuccess_DoesNotChangeVisitors()
    {
        var booking = Booking.Create(2, 1, new List<string>());

        booking.Close(CloseBookingReason.PaymentSuccess);

        Assert.Equal(2, booking.StandardVisitors);
        Assert.Equal(1, booking.DiscountVisitors);
    }

    [Fact]
    public void Close_PaymentFailed_SetsClosedStatusAndFailedPayment()
    {
        var booking = Booking.Create(2, 1, new List<string>());

        booking.Close(CloseBookingReason.PaymentFailed);

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Failed, booking.PaymentStatus);
    }

    [Fact]
    public void Close_PaymentFailed_ReleasesSeatsAndZerosVisitors()
    {
        var booking = Booking.Create(2, 1, new List<string> { "2", "3", "4" });
        booking.SeatNumbers.AddRange(new[] { "2", "3", "4" });

        booking.Close(CloseBookingReason.PaymentFailed);

        Assert.Empty(booking.SeatNumbers);
        Assert.Equal(0, booking.StandardVisitors);
        Assert.Equal(0, booking.DiscountVisitors);
    }
}
