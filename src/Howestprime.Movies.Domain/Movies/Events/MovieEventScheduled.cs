using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieEventScheduled(
    MovieEventId movieEventId,
    MovieId movieId,
    RoomId roomId,
    DateTime showTime
    ) : BaseDomainEvent(
        eventName: nameof(MovieEventScheduled),
        aggregateName: nameof(MovieEvent))
{
    public MovieEventId MovieEventId { get; private init; } = movieEventId;
    public MovieId MovieId { get; private init; } = movieId;
    public RoomId RoomId { get; private init; } = roomId;
    public DateTime ShowTime { get; private init; } = showTime;
}

