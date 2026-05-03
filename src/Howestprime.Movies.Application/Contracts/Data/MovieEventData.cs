namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record MovieEventData
{
    public Guid Id { get; init; }
    public DateTime ShowTime { get; init; }
    public int Capacity { get; init; }
    public required RoomData Room { get; init; }
    public required MovieData Movie { get; init; }
}
