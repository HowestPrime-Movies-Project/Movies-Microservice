using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public class BookingTests
{
    [Fact]
    public void Create_WithValidData_ReturnsBooking()
    {
        int standardVisitors = 2;
        int discountVisitors = 1;
        var booking = Booking.Create(standardVisitors, discountVisitors, new List<string>());
        Assert.NotNull(booking);
        Assert.NotEqual(Guid.Empty, booking.Id.Value);
        Assert.Equal(BookingStatus.Open, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Pending, booking.PaymentStatus);
        Assert.Equal(standardVisitors, booking.StandardVisitors);
        Assert.Equal(discountVisitors, booking.DiscountVisitors);
        Assert.Empty(booking.SeatNumbers);
    }

    [Fact]
    public void ValidateState_TotalVisitorsZero_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => Booking.Create(0, 0, new List<string>()));
        Assert.Equal("The total number of visitors must be greater than 0.", exception.Message);
    }

    [Fact]
    public void ValidateState_NegativeStandardVisitors_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => Booking.Create(-1, 1, new List<string>()));
        Assert.Equal("Standard visitors cannot be negative.", exception.Message);
    }

    [Fact]
    public void ValidateState_NegativeDiscountVisitors_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => Booking.Create(1, -1, new List<string>()));
        Assert.Equal("Discount visitors cannot be negative.", exception.Message);
    }

    [Fact]
    public void EmptyConstructor_CreatesObject()
    {
        var booking = new Booking();
        Assert.NotNull(booking);
    }

    [Fact]
    public void Constructor_BookingIdCoverage()
    {
        var id = new BookingId(Guid.NewGuid());
        Assert.NotEqual(Guid.Empty, id.Value);
    }
}
