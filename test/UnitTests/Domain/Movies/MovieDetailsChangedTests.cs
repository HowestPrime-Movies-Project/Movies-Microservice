using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public class MovieDetailsChangedTests
{
    private static Movie CreateMovie() => Movie.Create(
        "The Matrix",
        "A hacker discovers reality is simulated.",
        ReleaseYear.From(1999),
        Duration.From(136),
        [new Genre { Value = "Action" }],
        [new Actor { Value = "Keanu Reeves" }],
        AgeRating.From(16),
        PosterUrl.From("https://example.com/matrix.jpg")
    );

    [Fact]
    public void ChangeDetails_WithValidData_RaisesMovieDetailsChangedEvent()
    {
        var movie = CreateMovie();
        movie.ClearDomainEvents();

        movie.ChangeDetails(
            "Inception",
            "A mind-bending thriller",
            ReleaseYear.From(2010),
            Duration.From(148),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Leonardo DiCaprio" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/inception.jpg")
        );

        Assert.Single(movie.DomainEvents);
        var evt = Assert.IsType<MovieDetailsChanged>(movie.DomainEvents.Single());
        Assert.Equal(movie.Id, evt.MovieId);
        Assert.Equal("Inception", evt.Title);
        Assert.Equal("https://example.com/inception.jpg", evt.Poster);
        Assert.Equal(2010, evt.ReleaseYear);
        Assert.Equal(148, evt.Duration);
        Assert.Equal("Sci-Fi", evt.Genres);
        Assert.Equal("Leonardo DiCaprio", evt.Actors);
        Assert.Equal(13, evt.AgeRating);
    }

    [Fact]
    public void ChangeDetails_MultipleGenresAndActors_JoinsWithComma()
    {
        var movie = CreateMovie();
        movie.ClearDomainEvents();

        movie.ChangeDetails(
            "The Matrix",
            "A hacker discovers reality is simulated.",
            ReleaseYear.From(1999),
            Duration.From(136),
            [new Genre { Value = "Action" }, new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Keanu Reeves" }, new Actor { Value = "Laurence Fishburne" }],
            AgeRating.From(16),
            PosterUrl.From("https://example.com/matrix.jpg")
        );

        var evt = Assert.IsType<MovieDetailsChanged>(movie.DomainEvents.Single());
        Assert.Equal("Action,Sci-Fi", evt.Genres);
        Assert.Equal("Keanu Reeves,Laurence Fishburne", evt.Actors);
    }

    [Fact]
    public void MovieDetailsChanged_FQDN_IsCorrect()
    {
        var movie = CreateMovie();
        movie.ClearDomainEvents();

        movie.ChangeDetails(
            "Inception",
            "A mind-bending thriller",
            ReleaseYear.From(2010),
            Duration.From(148),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Leonardo DiCaprio" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/inception.jpg")
        );

        var evt = Assert.IsType<MovieDetailsChanged>(movie.DomainEvents.Single());
        Assert.Equal("Howestprime.Movies.Movie.MovieDetailsChanged", evt.FQDN);
    }
}
