using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieRegistered(
    MovieId movieId,
    string title,
    int releaseYear,
    int duration,
    List<string> genres,
    List<string> actors,
    int ageRating,
    string posterUrl
    ) : MovieDomainEvent(nameof(MovieRegistered))
{
    public MovieId MovieId { get; private init; } = movieId;
    public string Title { get; private init; } = title;
    public int ReleaseYear { get; private init; } = releaseYear;
    public int Duration { get; private init; } = duration;
    public List<string> Genres { get; private init; } = genres;
    public List<string> Actors { get; private init; } = actors;
    public int AgeRating { get; private init; } = ageRating;
    public string PosterUrl { get; private init; } = posterUrl;
}
