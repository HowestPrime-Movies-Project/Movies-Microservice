using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class MovieDomainEventTests
{
    [Fact]
    public void Constructor_ShouldSetFqdnForMovieAggregate()
    {
        // Arrange
        const string eventName = "Created";

        // Act
        MovieDomainEvent domainEvent = new(eventName);

        // Assert
        Assert.Equal("Howestprime.Movies.Movie.Created", domainEvent.FQDN);
    }

    [Fact]
    public void MovieRegistered_ShouldStorePayloadAndFqdn()
    {
        // Arrange
        MovieId movieId = new(Guid.NewGuid());

        // Act
        MovieRegistered domainEvent = new(
            movieId,
            "The Shawshank Redemption",
            1994,
            142,
            "Drama",
            "Tim Robbins,Morgan Freeman",
            16,
            "https://example.com/poster.jpg"
        );

        // Assert
        Assert.Equal(movieId, domainEvent.MovieId);
        Assert.Equal("The Shawshank Redemption", domainEvent.Title);
        Assert.Equal(1994, domainEvent.ReleaseYear);
        Assert.Equal(142, domainEvent.Duration);
        Assert.Equal("Drama", domainEvent.Genres);
        Assert.Equal("Tim Robbins,Morgan Freeman", domainEvent.Actors);
        Assert.Equal(16, domainEvent.AgeRating);
        Assert.Equal("https://example.com/poster.jpg", domainEvent.PosterUrl);
        Assert.Equal("Howestprime.Movies.Movie.MovieRegistered", domainEvent.FQDN);
    }
}
