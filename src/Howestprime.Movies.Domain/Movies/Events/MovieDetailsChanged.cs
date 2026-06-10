namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieDetailsChanged(
    MovieId movieId,
    string title,
    string poster,
    int releaseYear,
    int duration,
    string genres,
    string actors,
    int ageRating
) : MovieDomainEvent(nameof(MovieDetailsChanged))
{
    public MovieId MovieId { get; private init; } = movieId;
    public string Title { get; private init; } = title;
    public string Poster { get; private init; } = poster;
    public int ReleaseYear { get; private init; } = releaseYear;
    public int Duration { get; private init; } = duration;
    public string Genres { get; private init; } = genres;
    public string Actors { get; private init; } = actors;
    public int AgeRating { get; private init; } = ageRating;
}
