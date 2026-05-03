namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;

public sealed record Room
(
    Guid Id,
    string Name,
    int Capacity
);