using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;

public class RoomRepository(
    DomainDbContext context
    ) : EfCoreGenericRepository<Room, RoomId>(context), IRoomRepository
{
}

