using Aornis;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;

public class MovieEventRepository(
    DomainDbContext context
    ) : EfCoreGenericRepository<MovieEvent, MovieEventId>(context), IMovieEventRepository
{
    public async Task<bool> ExistsByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
    {
        return await _context
            .Set<MovieEvent>()
            .AnyAsync(me => me.ShowTime == showTime && me.RoomId.Equals(roomId));
    }
    
    public async Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
    {
        return await _context
            .Set<MovieEvent>()
            .FirstOrDefaultAsync(me => me.ShowTime == showTime && me.RoomId.Equals(roomId));
    }
}

