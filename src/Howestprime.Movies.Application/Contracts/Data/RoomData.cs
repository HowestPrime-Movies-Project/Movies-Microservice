namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record RoomData(
    Guid Id,
    string Name,
    int Capacity
    );