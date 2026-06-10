using Aornis;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public interface IMovieEventRepository : IRepository<MovieEvent, MovieEventId>
{
    Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId);
    Task<Optional<MovieEvent>> GetByBookingId(BookingId bookingId);
}



