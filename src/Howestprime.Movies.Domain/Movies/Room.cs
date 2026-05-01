using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct RoomId(Guid Value) : IEntityId;

public class Room : AggregateRoot<RoomId>
{
    public string Name { get; }
    public int Capacity { get; }
    
    public Room() { }
    
    private Room(RoomId id, string name, int capacity) : base(id)
    {
        Name = name;
        Capacity = capacity;
    }
    
    public static Room Create(string name, int capacity)
    {
        RoomId id = EntityId.New<RoomId>();
        Room room = new Room(id, name, capacity);
        room.ValidateState();
        room.RaiseDomainEvent(new RoomCreated(room.Id, room.Name, room.Capacity));
        return room;
    }
    
    public static Room Create(string name, int capacity, RoomId id)
    {
        Room room = new Room(id, name, capacity);
        room.ValidateState();
        room.RaiseDomainEvent(new RoomCreated(room.Id, room.Name, room.Capacity));
        return room;
    }
    
    public override void ValidateState()
    {
        Asserts.EnsureNotEmpty(Name);
        Asserts.EnsureGreaterThan(Capacity, 0);
    }
}
