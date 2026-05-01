using Aornis;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public interface IMovieEventRepository : IRepository<MovieEvent, MovieEventId> 
{
    Task<bool> ExistsByShowtimeAndRoomId(DateTime showTime, RoomId roomId);
    Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId);
}




