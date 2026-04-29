using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class MovieValueObjectTests
{
    [Fact]
    public void Actor_ShouldStoreValue()
    {
        // Arrange
        const string expected = "Keanu Reeves";

        // Act
        Actor actor = new() { Value = expected };

        // Assert
        Assert.Equal(expected, actor.Value);
    }

    [Fact]
    public void Genre_ShouldStoreValue()
    {
        // Arrange
        const string expected = "Action";

        // Act
        Genre genre = new() { Value = expected };

        // Assert
        Assert.Equal(expected, genre.Value);
    }

    [Fact]
    public void AgeRating_FromShouldStoreAge()
    {
        // Arrange
        const int expected = 16;

        // Act
        AgeRating ageRating = AgeRating.From(expected);

        // Assert
        Assert.Equal(expected, ageRating.Age);
    }

    [Fact]
    public void Duration_FromShouldStoreRuntime()
    {
        // Arrange
        const int expected = 136;

        // Act
        Duration duration = Duration.From(expected);

        // Assert
        Assert.Equal(expected, duration.Runtime);
    }

    [Fact]
    public void PosterUrl_FromShouldStoreUrl()
    {
        // Arrange
        const string expected = "https://example.com/poster.jpg";

        // Act
        PosterUrl posterUrl = PosterUrl.From(expected);

        // Assert
        Assert.Equal(expected, posterUrl.Url);
    }

    [Fact]
    public void ReleaseYear_FromShouldStoreYear()
    {
        // Arrange
        const int expected = 1999;

        // Act
        ReleaseYear releaseYear = ReleaseYear.From(expected);

        // Assert
        Assert.Equal(expected, releaseYear.Year);
    }
}
