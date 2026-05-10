using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct MovieEventId(Guid Value) : IEntityId;

public class MovieEvent : AggregateRoot<MovieEventId>
{ 
    public MovieId MovieId { get; } 
    public RoomId RoomId { get; } 
    public DateTime ShowTime { get; } 
    public int Capacity { get; } 
    public List<Booking> Bookings { get; } 
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
    
    public override void ValidateState() 
    {
        MovieEventAssertions.EnsureShowtimeIsAt15hOr19h(ShowTime); 
        MovieEventAssertions.EnsureShowtimeIsInTheFuture(ShowTime); 
        Asserts.EnsureGreaterThan(Capacity, 0);
        
    }
    
    public Booking Book(int standardVisitors, int discountVisitors) 
    { 
        Booking booking = Booking.Create(standardVisitors, discountVisitors); 
        int totalVisitors = Visitors + standardVisitors + discountVisitors; 
        if (totalVisitors > Capacity) throw new InvalidOperationException("Cannot book more visitors than the capacity of the movie event."); 
        Bookings.Add(booking); 
        Visitors = totalVisitors; 
        RaiseDomainEvent(new BookingOpened(booking.Id, MovieId, RoomId.Value.ToString(), ShowTime.ToString("o"), standardVisitors, discountVisitors, new List<string>())); 
        return booking; 
    }
}