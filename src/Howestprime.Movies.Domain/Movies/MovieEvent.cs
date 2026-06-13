using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct MovieEventId(Guid Value) : IEntityId;

public class MovieEvent : AggregateRoot<MovieEventId>
{
    public MovieId MovieId { get; private set; }
    public RoomId RoomId { get; }
    public DateTime ShowTime { get; }
    public int Capacity { get; }
    public List<Booking> Bookings { get; } = [];
    public int Visitors { get; private set; }
    
    public MovieEvent(){}
    
    private MovieEvent( 
        MovieEventId id, 
        MovieId movieId, 
        RoomId roomId, 
        DateTime showTime, 
        int capacity, 
        List<Booking> bookings, 
        int visitors 
        ) : base(id) 
    {
        MovieId = movieId; 
        RoomId = roomId; 
        ShowTime = showTime; 
        Capacity = capacity; 
        Bookings = bookings; 
        Visitors = visitors;
        
    }
    
    public static MovieEvent Create(MovieId movieId, RoomId roomId, DateTime showTime, int capacity) 
    { 
        // convert the showTime to UTC 
        MovieEventId id = EntityId.New<MovieEventId>(); 
        MovieEvent movieEvent = new MovieEvent(id, movieId, roomId, showTime, capacity, new List<Booking>(), 0); 
        movieEvent.ValidateState(); 
        movieEvent.RaiseDomainEvent(new MovieEventScheduled(movieEvent.Id, movieEvent.MovieId, movieEvent.RoomId, movieEvent.ShowTime)); 
        return movieEvent; 
    }
    
    public void ReassignMovie(MovieId movieId)
    {
        MovieId = movieId;
    }

    public override void ValidateState()
    {
        MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(ShowTime); 
        MovieEventAssertions.EnsureShowtimeIsInTheFuture(ShowTime); 
        Asserts.EnsureGreaterThan(Capacity, 0);
        
    }
    
    public Booking Book(int standardVisitors, int discountVisitors, string? roomName = null) 
    { 
        int totalVisitors = Visitors + standardVisitors + discountVisitors; 
        if (standardVisitors + discountVisitors <= 0) throw new InvalidOperationException("The total number of visitors must be greater than 0.");
        if (totalVisitors > Capacity) throw new InvalidOperationException("Cannot book more visitors than the capacity of the movie event."); 

        if (ShowTime > DateTime.UtcNow.AddDays(14))
            throw new InvalidOperationException("Cannot book movie events scheduled more than 14 days in advance.");

        int firstSeat = Visitors + 2;
        List<string> seatNumbers = Enumerable.Range(firstSeat, standardVisitors + discountVisitors)
            .Select(seatNumber => seatNumber.ToString())
            .ToList();

        Booking booking = Booking.Create(standardVisitors, discountVisitors, seatNumbers); 
        Bookings.Add(booking); 
        Visitors = totalVisitors; 
        RaiseDomainEvent(new BookingOpened(booking.Id, MovieId, roomName ?? RoomId.Value.ToString(), ShowTime.ToString("o"), standardVisitors, discountVisitors, seatNumbers));
        return booking;
    }

    public void CloseBooking(BookingId bookingId, CloseBookingReason reason)
    {
        Booking booking = Bookings.FirstOrDefault(b => b.Id == bookingId)
            ?? throw new InvalidOperationException($"Booking with ID {bookingId.Value} not found.");

        int visitorsToRelease = booking.StandardVisitors + booking.DiscountVisitors;
        booking.Close(reason);

        if (reason == CloseBookingReason.PaymentFailed)
            Visitors -= visitorsToRelease;
    }
}