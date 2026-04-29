using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public sealed class RegisterMovieTests
{
    [Fact]
    public async Task Execute_WithValidInput_ShouldSaveDoAndReturnId()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<RegisterMovie>();

        var usecase = new RegisterMovie(uow, logger);

        var input = new RegisterMovieInput(
            title: "Inception",
            description: "A mind-bending thriller",
            duration: 148,
            genres: new List<string> { "Sci-Fi", "Thriller" },
            actors: new List<string> { "Leonardo DiCaprio" },
            releaseYear: DateTime.Now.Year - 1,
            ageRating: 16,
            posterUrl: "https://example.com/inception.jpg"
        );

        // Act
        RegisterMovieOutput output = await usecase.Execute(input);

        // Assert
        Assert.True(uow.SaveCalled, "Save should have been called");
        Assert.Equal(typeof(IMovieRepository), uow.SaveRepositoryType);
        Assert.IsType<Movie>(uow.SavedAggregate);
        Assert.True(uow.DoCalled, "Do should have been called");

        // output should be a guid string
        Assert.True(Guid.TryParse(output.movieId, out _));

        // logger should have recorded an information message containing the id
        Assert.Contains(logger.Messages, m => m.Contains("Movie with ID"));
    }

    [Fact]
    public async Task Execute_WhenSaveThrows_ShouldPropagateException()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        uow.SaveAction = () => Task.FromException(new InvalidOperationException("Save failed"));
        var logger = new TestLogger<RegisterMovie>();

        var usecase = new RegisterMovie(uow, logger);

        var input = new RegisterMovieInput(
            title: "Inception",
            description: "A mind-bending thriller",
            duration: 148,
            genres: new List<string> { "Sci-Fi", "Thriller" },
            actors: new List<string> { "Leonardo DiCaprio" },
            releaseYear: DateTime.Now.Year - 1,
            ageRating: 16,
            posterUrl: "https://example.com/inception.jpg"
        );

        // Act
        Task act() => usecase.Execute(input);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }

    // Test doubles
    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public bool SaveCalled { get; private set; }
        public Type? SaveRepositoryType { get; private set; }
        public IAggregateRoot? SavedAggregate { get; private set; }
        public bool DoCalled { get; private set; }
        public Func<Task>? SaveAction { get; set; }

        public Task Do()
        {
            DoCalled = true;
            return Task.CompletedTask;
        }

        public Task Save<TRepository>(IAggregateRoot aggregateRoot) where TRepository : IRepository
        {
            SaveCalled = true;
            SaveRepositoryType = typeof(TRepository);
            SavedAggregate = aggregateRoot;

            if (SaveAction != null) return SaveAction();
            return Task.CompletedTask;
        }

        public TRepository Repo<TRepository>() where TRepository : IRepository => throw new NotImplementedException();
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

