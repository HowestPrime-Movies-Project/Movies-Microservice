using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public class MovieChangeDetailsTests
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
    public void ChangeDetails_WithValidData_UpdatesAllProperties()
    {
        var movie = CreateMovie();
        var newGenres = new List<Genre> { new Genre { Value = "Sci-Fi" } };
        var newActors = new List<Actor> { new Actor { Value = "Leonardo DiCaprio" } };

        movie.ChangeDetails(
            "Inception",
            "A mind-bending thriller",
            ReleaseYear.From(2010),
            Duration.From(148),
            newGenres,
            newActors,
            AgeRating.From(13),
            PosterUrl.From("https://example.com/inception.jpg")
        );

        Assert.Equal("Inception", movie.Title);
        Assert.Equal("A mind-bending thriller", movie.Description);
        Assert.Equal(2010, movie.ReleaseYear.Year);
        Assert.Equal(148, movie.Duration.Runtime);
        Assert.Same(newGenres, movie.Genres);
        Assert.Same(newActors, movie.Actors);
        Assert.Equal(13, movie.AgeRating.Age);
        Assert.Equal("https://example.com/inception.jpg", movie.PosterUrl.Url);
    }

    [Fact]
    public void ChangeDetails_WithEmptyTitle_ThrowsArgumentException()
    {
        var movie = CreateMovie();

        Assert.Throws<ArgumentException>(() => movie.ChangeDetails(
            string.Empty,
            "description",
            ReleaseYear.From(2010),
            Duration.From(148),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Actor" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/poster.jpg")
        ));
    }

    [Fact]
    public void ChangeDetails_WithEmptyDescription_ThrowsArgumentException()
    {
        var movie = CreateMovie();

        Assert.Throws<ArgumentException>(() => movie.ChangeDetails(
            "Title",
            string.Empty,
            ReleaseYear.From(2010),
            Duration.From(148),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Actor" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/poster.jpg")
        ));
    }

    [Fact]
    public void ChangeDetails_WithFutureReleaseYear_ThrowsArgumentException()
    {
        var movie = CreateMovie();

        Assert.Throws<ArgumentException>(() => movie.ChangeDetails(
            "Title",
            "Description",
            ReleaseYear.From(DateTime.Now.Year + 1),
            Duration.From(148),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Actor" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/poster.jpg")
        ));
    }

    [Fact]
    public void ChangeDetails_WithNegativeDuration_ThrowsArgumentException()
    {
        var movie = CreateMovie();

        Assert.Throws<ArgumentException>(() => movie.ChangeDetails(
            "Title",
            "Description",
            ReleaseYear.From(2010),
            Duration.From(-1),
            [new Genre { Value = "Sci-Fi" }],
            [new Actor { Value = "Actor" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/poster.jpg")
        ));
    }

    [Fact]
    public void ChangeDetails_WithNullGenres_ThrowsArgumentException()
    {
        var movie = CreateMovie();

        Assert.Throws<ArgumentException>(() => movie.ChangeDetails(
            "Title",
            "Description",
            ReleaseYear.From(2010),
            Duration.From(148),
            null!,
            [new Actor { Value = "Actor" }],
            AgeRating.From(13),
            PosterUrl.From("https://example.com/poster.jpg")
        ));
    }
}
