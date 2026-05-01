using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;

namespace UnitTests.Application;

public sealed class MovieDataFiltersTests
{
    [Fact]
    public void ByTitleAndGenres_WithRequestedGenres_AndNullMovieGenres_ShouldNotThrowAndReturnFalse()
    {
        var predicate = MovieDataFilters.ByTitleAndGenres(
            titleContains: null,
            genres: new List<string> { "sci-fi" }
        ).Compile();

        var movie = new MovieData
        {
            Title = "Inception",
            Description = "A mind-bending thriller",
            ReleaseYear = 2010,
            Duration = 148,
            Genres = null!,
            Actors = new List<ActorData>(),
            AgeRating = 16,
            PosterUrl = "https://example.com/inception.jpg"
        };

        var result = predicate(movie);

        Assert.False(result);
    }

    [Fact]
    public void ByTitleAndGenres_WithMixedCaseTitleAndGenres_ShouldReturnTrue()
    {
        var predicate = MovieDataFilters.ByTitleAndGenres(
            titleContains: "INCEPTION",
            genres: new List<string> { "SCI-FI" }
        ).Compile();

        var movie = new MovieData
        {
            Title = "Inception",
            Description = "A mind-bending thriller",
            ReleaseYear = 2010,
            Duration = 148,
            Genres = new List<GenreData> { new("sci-fi"), new("thriller") },
            Actors = new List<ActorData>(),
            AgeRating = 16,
            PosterUrl = "https://example.com/inception.jpg"
        };

        Assert.True(predicate(movie));
    }

    [Fact]
    public void ByTitleAndGenres_WithNullGenres_ShouldNotThrowAndReturnTrue()
    {
        var predicate = MovieDataFilters.ByTitleAndGenres(
            titleContains: "Inception",
            genres: null
        ).Compile();

        var movie = new MovieData
        {
            Title = "Inception",
            Description = "A mind-bending thriller",
            ReleaseYear = 2010,
            Duration = 148,
            Genres = new List<GenreData> { new("sci-fi"), new("thriller") },
            Actors = new List<ActorData>(),
            AgeRating = 16,
            PosterUrl = "https://example.com/inception.jpg"
        };

        var result = predicate(movie);

        Assert.True(result);
    }
}
