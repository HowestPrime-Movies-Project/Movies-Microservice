using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct BookingId(Guid Value) : IEntityId;


public sealed class Booking : Entity<BookingId>
{
    public BookingStatus BookingStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public int StandardVisitors { get; private set; }
    public int DiscountVisitors { get; private set; }
    public List<string> SeatNumbers { get; }

    public Booking() {}
    private Booking(
        BookingId id,
        BookingStatus bookingStatus,
        PaymentStatus paymentStatus,
        int standardVisitors,
        int discountVisitors,
        List<string> seatNumbers
    ) : base(id: id)
    {
        BookingStatus = bookingStatus;
        PaymentStatus = paymentStatus;
        StandardVisitors = standardVisitors;
        DiscountVisitors = discountVisitors;
        SeatNumbers = seatNumbers;
    }

    public static Booking Create(
        int standardVisitors,
        int discountVisitors,
        List<string> seatNumbers
        )
    {
        BookingId id = EntityId.New<BookingId>();
        Booking booking = new Booking(id, BookingStatus.Open, PaymentStatus.Pending, standardVisitors, discountVisitors, new List<string>());
        booking.ValidateState();
        return booking;
    }

    public void Close(CloseBookingReason reason)
    {
        BookingStatus = BookingStatus.Closed;
        PaymentStatus = reason == CloseBookingReason.PaymentSuccess ? PaymentStatus.Succes : PaymentStatus.Failed;

        if (reason == CloseBookingReason.PaymentFailed)
        {
            SeatNumbers.Clear();
            StandardVisitors = 0;
            DiscountVisitors = 0;
        }
    }

    public override void ValidateState()
    {
        if (StandardVisitors < 0) throw new ArgumentException("Standard visitors cannot be negative.");
        if (DiscountVisitors < 0) throw new ArgumentException("Discount visitors cannot be negative.");
        if (0 >= StandardVisitors + DiscountVisitors) throw new ArgumentException("The total number of visitors must be greater than 0.");
    }
}
