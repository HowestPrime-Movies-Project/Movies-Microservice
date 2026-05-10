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
    public void Actor_CopyConstructor_ShouldPreserveValue()
    {
        // Arrange
        const string value = "Tom Hanks";
        Actor original = new() { Value = value };

        // Act
        Actor copy = original with { };

        // Assert
        Assert.Equal(value, copy.Value);
        Assert.Equal(original, copy);
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
    public void Genre_CopyConstructor_ShouldPreserveValue()
    {
        // Arrange
        const string value = "Comedy";
        Genre original = new() { Value = value };

        // Act
        Genre copy = original with { };

        // Assert
        Assert.Equal(value, copy.Value);
        Assert.Equal(original, copy);
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
    public void AgeRating_CopyConstructor_ShouldPreserveAge()
    {
        // Arrange
        const int age = 18;
        AgeRating original = AgeRating.From(age);

        // Act
        AgeRating copy = original with { };

        // Assert
        Assert.Equal(age, copy.Age);
        Assert.Equal(original, copy);
    }

    [Fact]
    public void AgeRating_DirectInitialization()
    {
        // Arrange & Act
        AgeRating ageRating = new() { Age = 12 };

        // Assert
        Assert.Equal(12, ageRating.Age);
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
    public void Duration_CopyConstructor_ShouldPreserveRuntime()
    {
        // Arrange
        const int runtime = 120;
        Duration original = Duration.From(runtime);

        // Act
        Duration copy = original with { };

        // Assert
        Assert.Equal(runtime, copy.Runtime);
        Assert.Equal(original, copy);
    }

    [Fact]
    public void Duration_DirectInitialization()
    {
        // Arrange & Act
        Duration duration = new() { Runtime = 95 };

        // Assert
        Assert.Equal(95, duration.Runtime);
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
    public void PosterUrl_CopyConstructor_ShouldPreserveUrl()
    {
        // Arrange
        const string url = "https://example.com/movie.jpg";
        PosterUrl original = PosterUrl.From(url);

        // Act
        PosterUrl copy = original with { };

        // Assert
        Assert.Equal(url, copy.Url);
        Assert.Equal(original, copy);
    }

    [Fact]
    public void PosterUrl_DirectInitialization()
    {
        // Arrange & Act
        PosterUrl posterUrl = new() { Url = "https://example.com/test.jpg" };

        // Assert
        Assert.Equal("https://example.com/test.jpg", posterUrl.Url);
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

    [Fact]
    public void ReleaseYear_CopyConstructor_ShouldPreserveYear()
    {
        // Arrange
        const int year = 2023;
        ReleaseYear original = ReleaseYear.From(year);

        // Act
        ReleaseYear copy = original with { };

        // Assert
        Assert.Equal(year, copy.Year);
        Assert.Equal(original, copy);
    }

    [Fact]
    public void ReleaseYear_DirectInitialization()
    {
        // Arrange & Act
        ReleaseYear releaseYear = new() { Year = 2010 };

        // Assert
        Assert.Equal(2010, releaseYear.Year);
    }

    [Fact]
    public void Booking_CopyConstructor_ShouldPreserveValue()
    {
        // Arrange
        Booking original = new();

        // Act
        Booking copy = original;

        // Assert
        Assert.Equal(original, copy);
    }

    [Fact]
    public void Actor_WithMultipleValues_ShouldMaintainInequality()
    {
        // Arrange
        Actor actor1 = new() { Value = "Actor One" };
        Actor actor2 = new() { Value = "Actor Two" };

        // Act & Assert
        Assert.NotEqual(actor1, actor2);
    }

    [Fact]
    public void Genre_WithMultipleValues_ShouldMaintainInequality()
    {
        // Arrange
        Genre genre1 = new() { Value = "Action" };
        Genre genre2 = new() { Value = "Drama" };

        // Act & Assert
        Assert.NotEqual(genre1, genre2);
    }

    [Fact]
    public void AgeRating_WithDifferentAges_ShouldMaintainInequality()
    {
        // Arrange
        AgeRating rating1 = AgeRating.From(12);
        AgeRating rating2 = AgeRating.From(18);

        // Act & Assert
        Assert.NotEqual(rating1, rating2);
    }

    [Fact]
    public void Duration_WithDifferentRuntimes_ShouldMaintainInequality()
    {
        // Arrange
        Duration duration1 = Duration.From(100);
        Duration duration2 = Duration.From(150);

        // Act & Assert
        Assert.NotEqual(duration1, duration2);
    }

    [Fact]
    public void PosterUrl_WithDifferentUrls_ShouldMaintainInequality()
    {
        // Arrange
        PosterUrl url1 = PosterUrl.From("https://example.com/one.jpg");
        PosterUrl url2 = PosterUrl.From("https://example.com/two.jpg");

        // Act & Assert
        Assert.NotEqual(url1, url2);
    }

    [Fact]
    public void ReleaseYear_WithDifferentYears_ShouldMaintainInequality()
    {
        // Arrange
        ReleaseYear year1 = ReleaseYear.From(1990);
        ReleaseYear year2 = ReleaseYear.From(2000);

        // Act & Assert
        Assert.NotEqual(year1, year2);
    }
}
