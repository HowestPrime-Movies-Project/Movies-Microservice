using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public class BookingOpenedTests
{
    [Fact]
    public void Constructing_BookingOpened_ShouldSetProperties()
    {
        var bookingId = new BookingId(Guid.NewGuid());
        var movieId = new MovieId(Guid.NewGuid());
        var room = "Room 3";
        var showTime = "2026-05-10T20:00:00Z";
        var seatNumbers = new List<string> { "A1" };
        var eventObj = new BookingOpened(bookingId, movieId, room, showTime, 2, 1, seatNumbers);
        
        Assert.NotNull(eventObj);
        Assert.Equal(showTime, eventObj.ShowTime);
        Assert.Equal(2, eventObj.StandardVisitors);
        Assert.Equal(1, eventObj.DiscountedVisitors);
        Assert.Equal(seatNumbers, eventObj.SeatNumbers);
        
        // Using reflection to check private init properties
        var prop1 = typeof(BookingOpened).GetProperty("BookingId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        var prop2 = typeof(BookingOpened).GetProperty("MovieId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        var prop3 = typeof(BookingOpened).GetProperty("Room", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        
        Assert.Equal(bookingId, prop1?.GetValue(eventObj));
        Assert.Equal(movieId, prop2?.GetValue(eventObj));
        Assert.Equal(room, prop3?.GetValue(eventObj));
    }
}
