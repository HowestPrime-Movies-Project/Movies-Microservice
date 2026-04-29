using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieRegistered(
    MovieId movieId,
    string title,
    ReleaseYear releaseYear
    ) : MovieDomainEvent(nameof(MovieRegistered))
{
    public MovieId MovieId { get; private init; } = movieId;
    public string Title { get; private init; } = title;
    public ReleaseYear ReleaseYear { get; private init; } = releaseYear;
}