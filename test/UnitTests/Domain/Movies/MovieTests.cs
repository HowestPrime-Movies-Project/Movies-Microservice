using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class MovieTests
{
    [Fact]
    public void DefaultConstructor_ShouldLeavePropertiesUninitialized()
    {
        // Arrange & Act
        Movie movie = new();

        // Assert
        Assert.Equal(default, movie.Id);
        Assert.Null(movie.Title);
        Assert.Null(movie.Description);
        Assert.Null(movie.ReleaseYear);
        Assert.Null(movie.Duration);
        Assert.Null(movie.Genres);
        Assert.Null(movie.Actors);
        Assert.Null(movie.AgeRating);
        Assert.Null(movie.PosterUrl);
        Assert.Empty(movie.DomainEvents);
    }

    [Fact]
    public void Create_WithValidState_ShouldMapPropertiesAndRaiseEvent()
    {
        // Arrange
        string title = "The Matrix";
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(136);
        List<Genre> genres = [new Genre { Value = "Action" }, new Genre { Value = "Sci-Fi" }];
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }, new Actor { Value = "Carrie-Anne Moss" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");

        // Act
        Movie movie = Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.NotEqual(default, movie.Id);
        Assert.Equal(title, movie.Title);
        Assert.Equal(description, movie.Description);
        Assert.Equal(releaseYear, movie.ReleaseYear);
        Assert.Equal(duration, movie.Duration);
        Assert.Same(genres, movie.Genres);
        Assert.Same(actors, movie.Actors);
        Assert.Equal(ageRating, movie.AgeRating);
        Assert.Equal(posterUrl, movie.PosterUrl);
        Assert.Single(movie.DomainEvents);

        Howestprime.Movies.Domain.Movies.Events.MovieRegistered registered =
            Assert.IsType<Howestprime.Movies.Domain.Movies.Events.MovieRegistered>(movie.DomainEvents.Single());
        Assert.Equal(movie.Id, registered.MovieId);
        Assert.Equal(title, registered.Title);
        Assert.Equal(releaseYear, registered.ReleaseYear);
        Assert.Equal("Howestprime.Movies.Movie.MovieRegistered", registered.FQDN);
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldThrow()
    {
        // Arrange
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(136);
        List<Genre> genres = [new Genre { Value = "Action" }];
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");

        // Act
        Action act = () => Movie.Create(string.Empty, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithEmptyDescription_ShouldThrow()
    {
        // Arrange
        string title = "The Matrix";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(136);
        List<Genre> genres = [new Genre { Value = "Action" }];
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");

        // Act
        Action act = () => Movie.Create(title, string.Empty, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNullGenres_ShouldThrow()
    {
        // Arrange
        string title = "The Matrix";
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(136);
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");
        List<Genre>? genres = null;

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres!, actors, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNullActors_ShouldThrow()
    {
        // Arrange
        string title = "The Matrix";
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(136);
        List<Genre> genres = [new Genre { Value = "Action" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");
        List<Actor>? actors = null;

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors!, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeDuration_ShouldThrow()
    {
        // Arrange
        string title = "The Matrix";
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(DateTime.Now.Year - 1);
        Duration duration = Duration.From(-1);
        List<Genre> genres = [new Genre { Value = "Action" }];
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithFutureReleaseYear_ShouldThrow()
    {
        // Arrange
        int currentYear = DateTime.Now.Year;
        string title = "The Matrix";
        string description = "A hacker discovers reality is simulated.";
        ReleaseYear releaseYear = ReleaseYear.From(currentYear + 1);
        Duration duration = Duration.From(136);
        List<Genre> genres = [new Genre { Value = "Action" }];
        List<Actor> actors = [new Actor { Value = "Keanu Reeves" }];
        AgeRating ageRating = AgeRating.From(16);
        PosterUrl posterUrl = PosterUrl.From("https://example.com/matrix.jpg");

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}
