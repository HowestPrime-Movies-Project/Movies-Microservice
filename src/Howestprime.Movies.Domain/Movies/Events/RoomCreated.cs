using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class RoomCreated(
    RoomId roomId,
    string name,
    int capacity
    ) : BaseDomainEvent(
        eventName: nameof(RoomCreated),
        aggregateName: nameof(Room))
{
    public RoomId RoomId { get; private init; } = roomId;
    public string Name { get; private init; } = name;
    public int Capacity { get; private init; } = capacity;
}

