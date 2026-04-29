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
        ReleaseYear releaseYear = ReleaseYear.From(1994);

        // Act
        MovieRegistered domainEvent = new(movieId, "The Shawshank Redemption", releaseYear);

        // Assert
        Assert.Equal(movieId, domainEvent.MovieId);
        Assert.Equal("The Shawshank Redemption", domainEvent.Title);
        Assert.Equal(releaseYear, domainEvent.ReleaseYear);
        Assert.Equal("Howestprime.Movies.Movie.MovieRegistered", domainEvent.FQDN);
    }
}
