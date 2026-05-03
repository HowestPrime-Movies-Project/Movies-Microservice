namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;

public sealed record MovieEvent
(
    Guid Id,
    DateTime Showtime,
    int Capacity,
    Room Room,
    Movie Movie
);