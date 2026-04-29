using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct MovieId(Guid Value) : IEntityId;

public class Movie : AggregateRoot<MovieId>
{
    public string Title { get; }
    public string Description { get; }
    public ReleaseYear ReleaseYear { get; }
    public Duration Duration { get; }
    public IReadOnlyList<Genre> Genres { get; }
    public IReadOnlyList<Actor> Actors { get; }
    public AgeRating AgeRating { get; }
    public PosterUrl PosterUrl { get; }

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
            movie.ReleaseYear
            ));

        return movie;
    }

    public override void ValidateState()
    {
        Asserts.EnsureNotEmpty(Title);
        Asserts.EnsureNotEmpty(Description);
        Asserts.EnsureNotEmpty(Genres);
        Asserts.EnsureNotEmpty(Actors);
        Asserts.EnsureNotNegative(Duration.Runtime);
        Asserts.EnsureLessThan(ReleaseYear.Year, DateTime.Now.Year);
    }
}

