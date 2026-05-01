using Microsoft.Extensions.Logging;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Seeders;

public class DomainDbSeeder(DomainDbContext context, ILogger<DomainDbSeeder> _logger)
{
    public async Task Seed()
    {
        await SeedRooms();
    }
    
    private async Task SeedRooms()
    {
        // Check if rooms already exist
        if (context.Rooms.Any())
            return;

        var rooms = new List<Room>
        {
            Room.Create("Blue Room", 100, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-71db-8a1a-ec7569492999"))),
            Room.Create("Yellow Room", 80, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-75fe-b936-0a97cd75216e")))
        };

        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {RoomCount} rooms.", rooms.Count);
    }
}

