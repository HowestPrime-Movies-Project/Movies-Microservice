using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct MovieId(Guid Value) : IEntityId;

public class Movie : AggregateRoot<MovieId>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public ReleaseYear ReleaseYear { get; private set; }
    public Duration Duration { get; private set; }
    public IReadOnlyList<Genre> Genres { get; private set; }
    public IReadOnlyList<Actor> Actors { get; private set; }
    public AgeRating AgeRating { get; private set; }
    public PosterUrl PosterUrl { get; private set; }

    public Movie(){}
    private Movie(
        MovieId id,
        string title,
        string description,
        ReleaseYear releaseYear,
        Duration duration,
        List<Genre> genres,
        List<Actor> actors,
        AgeRating ageRating,
        PosterUrl posterUrl
    ) : base(id: id)
    {
        Title = title;
        Description = description;
        ReleaseYear = releaseYear;
        Duration = duration;
        Genres = genres;
        Actors = actors;
        AgeRating = ageRating;
        PosterUrl = posterUrl;
    }

    public static Movie Create(string title, string description, ReleaseYear releaseYear, Duration duration, List<Genre> genres, List<Actor> actors, AgeRating ageRating, PosterUrl posterUrl)
    {
        MovieId id = EntityId.New<MovieId>();
        Movie movie = new Movie(id, title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);
        movie.ValidateState();
        movie.RaiseDomainEvent(new MovieRegistered(
            movie.Id,
            movie.Title,
            movie.ReleaseYear.Year,
            movie.Duration.Runtime,
            movie.Genres.Select(g => g.Value).ToList(),
            movie.Actors.Select(a => a.Value).ToList(),
            movie.AgeRating.Age,
            movie.PosterUrl.Url
            ));

        return movie;
    }

    public void ChangeDetails(string title, string description, ReleaseYear releaseYear, Duration duration, List<Genre> genres, List<Actor> actors, AgeRating ageRating, PosterUrl posterUrl)
    {
        Title = title;
        Description = description;
        ReleaseYear = releaseYear;
        Duration = duration;
        Genres = genres;
        Actors = actors;
        AgeRating = ageRating;
        PosterUrl = posterUrl;

        ValidateState();

        RaiseDomainEvent(new MovieDetailsChanged(
            Id,
            title,
            posterUrl.Url,
            releaseYear.Year,
            duration.Runtime,
            string.Join(",", genres.Select(g => g.Value)),
            string.Join(",", actors.Select(a => a.Value)),
            ageRating.Age
        ));
    }

    public override void ValidateState()
    {
        Asserts.EnsureNotEmpty(Title);
        Asserts.EnsureNotEmpty(Description);
        Asserts.EnsureNotEmpty(Genres);
        Asserts.EnsureNotEmpty(Actors);
        Asserts.EnsureNotNegative(Duration.Runtime);
        Asserts.EnsureLessThanOrEqual(ReleaseYear.Year, DateTime.Now.Year);
    }
}
