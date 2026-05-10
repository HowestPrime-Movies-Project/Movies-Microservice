using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct BookingId(Guid Value) : IEntityId;


public sealed class Booking : Entity<BookingId>
{
    public BookingStatus BookingStatus { get; }
    public PaymentStatus PaymentStatus { get; }
    public int StandardVisitors { get; }
    public int DiscountVisitors { get; }
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
        int discountVisitors
        )
    {
        BookingId id = EntityId.New<BookingId>();
        Booking booking = new Booking(id, BookingStatus.Open, PaymentStatus.Pending, standardVisitors, discountVisitors, new List<string>());
        booking.ValidateState();
        return booking;
    }
    
    public override void ValidateState()
    {
        if (StandardVisitors < 0) throw new ArgumentException("Standard visitors cannot be negative.");
        if (DiscountVisitors < 0) throw new ArgumentException("Discount visitors cannot be negative.");
        if (0 != StandardVisitors + DiscountVisitors) throw new ArgumentException("The number of visitors must be greater than 0.");
    }
}
