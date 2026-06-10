using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public class ChangeMovieDetailsTests
{
    private readonly StubUnitOfWorkForChangeMovieDetails _uow;
    private readonly StubLogger<ChangeMovieDetails> _logger;
    private readonly ChangeMovieDetails _sut;

    public ChangeMovieDetailsTests()
    {
        _uow = new StubUnitOfWorkForChangeMovieDetails();
        _logger = new StubLogger<ChangeMovieDetails>();
        _sut = new ChangeMovieDetails(_uow, _logger);
    }

    private static ChangeMovieDetailsInput ValidInput(string movieId) => new(
        movieId,
        "Inception",
        "A mind-bending thriller",
        148,
        new List<string> { "Sci-Fi" },
        new List<string> { "Leonardo DiCaprio" },
        2010,
        13,
        "https://example.com/inception.jpg"
    );

    [Fact]
    public void InputRecord_Works()
    {
        var input = ValidInput(Guid.NewGuid().ToString());
        Assert.Equal("Inception", input.title);
        Assert.Equal(148, input.duration);
    }

    [Fact]
    public async Task Execute_InvalidGuid_ThrowsArgumentException()
    {
        var input = ValidInput("invalid-guid");

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.Execute(input));

        Assert.StartsWith("Invalid movie ID format", exception.Message);
    }

    [Fact]
    public async Task Execute_MovieNotFound_ThrowsInvalidOperationException()
    {
        var movieId = Guid.NewGuid();
        var input = ValidInput(movieId.ToString());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.Execute(input));

        Assert.Contains(movieId.ToString(), exception.Message);
    }

    [Fact]
    public async Task Execute_ValidInput_UpdatesMovieAndSaves()
    {
        var movie = Movie.Create(
            "The Matrix",
            "A hacker discovers reality is simulated.",
            ReleaseYear.From(1999),
            Duration.From(136),
            [new Genre { Value = "Action" }],
            [new Actor { Value = "Keanu Reeves" }],
            AgeRating.From(16),
            PosterUrl.From("https://example.com/matrix.jpg")
        );
        _uow.SetMovie(movie.Id, movie);

        await _sut.Execute(ValidInput(movie.Id.Value.ToString()));

        Assert.Equal("Inception", movie.Title);
        Assert.Equal(2010, movie.ReleaseYear.Year);
        Assert.True(_uow.SaveWasCalled);
        Assert.True(_uow.DoWasCalled);
    }

    [Fact]
    public async Task Execute_ValidInput_RaisesMovieDetailsChangedEvent()
    {
        var movie = Movie.Create(
            "The Matrix",
            "A hacker discovers reality is simulated.",
            ReleaseYear.From(1999),
            Duration.From(136),
            [new Genre { Value = "Action" }],
            [new Actor { Value = "Keanu Reeves" }],
            AgeRating.From(16),
            PosterUrl.From("https://example.com/matrix.jpg")
        );
        movie.ClearDomainEvents();
        _uow.SetMovie(movie.Id, movie);

        await _sut.Execute(ValidInput(movie.Id.Value.ToString()));

        Assert.Contains(movie.DomainEvents, e => e is MovieDetailsChanged);
    }
}

public class StubUnitOfWorkForChangeMovieDetails : IUnitOfWork
{
    private readonly Dictionary<MovieId, Movie> _movies = new();
    public bool SaveWasCalled { get; private set; }
    public bool DoWasCalled { get; private set; }

    public void SetMovie(MovieId id, Movie movie) => _movies[id] = movie;

    public TRepository Repo<TRepository>() where TRepository : IRepository
    {
        if (typeof(TRepository) == typeof(IMovieRepository))
            return (TRepository)(IRepository)new StubMovieRepository(_movies);

        throw new InvalidOperationException($"Unknown repository type: {typeof(TRepository)}");
    }

    public Task Save<TRepository>(IAggregateRoot aggregateRoot) where TRepository : IRepository
    {
        SaveWasCalled = true;
        return Task.CompletedTask;
    }

    public Task Do()
    {
        DoWasCalled = true;
        return Task.CompletedTask;
    }
}

public class StubMovieRepository(Dictionary<MovieId, Movie> movies) : IMovieRepository
{
    public Task<Optional<Movie>> ById(MovieId id)
    {
        movies.TryGetValue(id, out var movie);
        return Task.FromResult(Optional.Of(movie));
    }

    public Task<bool> Exists(MovieId id) => Task.FromResult(movies.ContainsKey(id));
    public Task Save(Movie aggregateRoot) => Task.CompletedTask;
    public Task Remove(Movie aggregateRoot) => Task.CompletedTask;
}
