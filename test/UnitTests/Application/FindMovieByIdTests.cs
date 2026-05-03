using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public sealed class FindMovieByIdTests
{
    [Fact]
    public async Task Execute_WithValidInput_ShouldAuthorizeAndFetch()
    {
        // Arrange
        var expectedMovie = new MovieData
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "The Matrix",
            Description = "A sci-fi action film",
            ReleaseYear = 1999,
            Duration = 136,
            Genres = new List<GenreData> { new("sci-fi") },
            Actors = new List<ActorData> { new("Keanu Reeves") },
            AgeRating = 16,
            PosterUrl = "https://example.com/matrix.jpg"
        };

        var query = new FakeFindMovieByIdQuery(expectedMovie);
        var authorization = new FakeAuthorizationService();
        var logger = new TestLogger<FindMovieById>();
        var useCase = new FindMovieById(query, authorization, logger);

        var input = new FindMovieByIdInput(
            Id: "33333333-3333-3333-3333-333333333333",
            xUserRole: "user"
        );

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.Equal(expectedMovie, result);
        Assert.Equal(("user", nameof(FindMovieById)), authorization.Calls.Single());
        var capturedFilter = query.CapturedFilter;
        Assert.NotNull(capturedFilter);

        var predicate = capturedFilter.Compile();
        Assert.True(predicate(expectedMovie));
        Assert.False(predicate(expectedMovie with { Id = Guid.Parse("44444444-4444-4444-4444-444444444444") }));
    }

    [Fact]
    public async Task Execute_WithNotFoundMovie_ShouldReturnNull()
    {
        // Arrange
        var query = new FakeFindMovieByIdQuery(null);
        var authorization = new FakeAuthorizationService();
        var logger = new TestLogger<FindMovieById>();
        var useCase = new FindMovieById(query, authorization, logger);

        var input = new FindMovieByIdInput(
            Id: "55555555-5555-5555-5555-555555555555",
            xUserRole: "admin"
        );

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.Null(result);
        Assert.Equal(("admin", nameof(FindMovieById)), authorization.Calls.Single());
    }

    [Fact]
    public void Records_ShouldExposeTheirValues()
    {
        var input = new FindMovieByIdInput("12345", "admin");

        Assert.Equal("12345", input.Id);
        Assert.Equal("admin", input.xUserRole);
    }

    private sealed class FakeFindMovieByIdQuery(MovieData? result) : IFindMovieByIdQuery
    {
        public Expression<Func<MovieData, bool>>? CapturedFilter { get; private set; }

        public Task<MovieData?> Fetch(Expression<Func<MovieData, bool>> movieFilter)
        {
            CapturedFilter = movieFilter;
            return Task.FromResult(result);
        }
    }

    private sealed class FakeAuthorizationService : IAuthorizationService
    {
        public List<(string Role, string Permission)> Calls { get; } = new();

        public void Authorize(string userRole, string requestedPermission)
        {
            Calls.Add((userRole, requestedPermission));
        }
    }
    
    private sealed class TestLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = new();

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }

        private sealed class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new NullScope();
            public void Dispose() { }
        }
    }

}

