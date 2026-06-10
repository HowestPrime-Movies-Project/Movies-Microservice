using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public class BookMovieEventTests
{
    private readonly StubUnitOfWork _uow;
    private readonly StubLogger<BookMovieEvent> _logger;
    private readonly BookMovieEvent _sut;

    public BookMovieEventTests()
    {
        _uow = new StubUnitOfWork();
        _logger = new StubLogger<BookMovieEvent>();
        _sut = new BookMovieEvent(_uow, _logger);
    }

    [Fact]
    public void InputOutput_Records_Work()
    {
        var input = new BookMovieEventInput("id", 2, 1);
        Assert.Equal("id", input.movieEventId);
        Assert.Equal(2, input.standardVisitors);
        Assert.Equal(1, input.discountVisitors);
        
        var output = new BookMovieEventOutput("bookingId");
        Assert.Equal("bookingId", output.bookingId);
    }

    [Fact]
    public async Task Execute_InvalidGuid_ThrowsArgumentException()
    {
        var input = new BookMovieEventInput("invalid-guid", 2, 1);
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.Execute(input));
        Assert.StartsWith("Invalid movie event ID format", exception.Message);
    }

    [Fact]
    public async Task Execute_EventNotFound_ThrowsInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var idString = id.ToString();
        var input = new BookMovieEventInput(idString, 2, 1);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.Execute(input));
        Assert.Equal($"Movie event with ID {id} not found.", exception.Message);
    }

    [Fact]
    public async Task Execute_ValidInput_BooksEventAndReturnsOutput()
    {
        var eventId = Guid.NewGuid();
        var movieId = Guid.NewGuid();
        var input = new BookMovieEventInput(eventId.ToString(), 2, 1);
        
        var movieEvent = MovieEvent.Create(
            new MovieId(movieId), 
            new RoomId(Guid.NewGuid()), 
            DateTime.UtcNow.AddDays(1).Date.Add(new TimeSpan(19, 0, 0)),
            10
        );
        
        _uow.SetMovieEvent(new MovieEventId(eventId), movieEvent);
        var result = await _sut.Execute(input);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result.bookingId);
        Assert.True(_uow.SaveWasCalled);
        Assert.True(_uow.DoWasCalled);
    }
}

// Stub implementations to replace Moq
public class StubUnitOfWork : IUnitOfWork
{
    private Dictionary<MovieEventId, MovieEvent> _movieEvents = new();
    public bool SaveWasCalled { get; private set; }
    public bool DoWasCalled { get; private set; }

    public void SetMovieEvent(MovieEventId id, MovieEvent movieEvent)
    {
        _movieEvents[id] = movieEvent;
    }

    public TRepository Repo<TRepository>() where TRepository : IRepository
    {
        if (typeof(TRepository) == typeof(IMovieEventRepository))
        {
            return (TRepository)(IRepository)new StubMovieEventRepository(_movieEvents);
        }
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

public class StubMovieEventRepository : IMovieEventRepository
{
    private readonly Dictionary<MovieEventId, MovieEvent> _movieEvents;

    public StubMovieEventRepository(Dictionary<MovieEventId, MovieEvent> movieEvents)
    {
        _movieEvents = movieEvents;
    }

    public async Task<Optional<MovieEvent>> ById(MovieEventId id)
    {
        if (_movieEvents.TryGetValue(id, out var movieEvent))
        {
            return await Task.FromResult(Optional.Of(movieEvent));
        }
        return await Task.FromResult(Optional.Of<MovieEvent>(null));
    }

    public async Task<bool> Exists(MovieEventId id)
    {
        return await Task.FromResult(_movieEvents.ContainsKey(id));
    }

    public async Task Save(MovieEvent aggregateRoot)
    {
        await Task.CompletedTask;
    }

    public async Task Remove(MovieEvent aggregateRoot)
    {
        await Task.CompletedTask;
    }

    public async Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
    {
        return await Task.FromResult<MovieEvent?>(null);
    }

    public async Task<Optional<MovieEvent>> GetByBookingId(BookingId bookingId)
    {
        return await Task.FromResult(Optional.Of<MovieEvent>(null));
    }
}

public class StubLogger<T> : ILogger<T>
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }

    public bool IsEnabled(LogLevel logLevel) => false;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}

