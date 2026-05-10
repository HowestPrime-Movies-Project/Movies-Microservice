using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class BookingOpened(
    BookingId bookingId,
    MovieId movieId,
    string room,
    string showTime,
    int standardVisitors,
    int discountedVisitors,
    List<string> seatNumbers
    ) : BaseDomainEvent(
    eventName: nameof(BookingOpened),
    aggregateName: nameof(MovieEvent))
{
    BookingId BookingId { get; init; } =  bookingId;
    MovieId MovieId { get; init; } = movieId;
    string Room { get; init; } = room;
    public string ShowTime { get; private init; } = showTime;
    public int StandardVisitors { get; private init; } = standardVisitors;
    public int DiscountedVisitors { get; private init; }  = discountedVisitors;
    public List<string> SeatNumbers { get; private init; } = seatNumbers;
}