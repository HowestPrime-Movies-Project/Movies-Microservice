using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public class CloseBookingTests
{
    private readonly StubUnitOfWorkForCloseBooking _uow;
    private readonly StubLogger<CloseBooking> _logger;
    private readonly CloseBooking _sut;

    public CloseBookingTests()
    {
        _uow = new StubUnitOfWorkForCloseBooking();
        _logger = new StubLogger<CloseBooking>();
        _sut = new CloseBooking(_uow, _logger);
    }

    [Fact]
    public void InputRecord_Works()
    {
        var input = new CloseBookingInput("some-id", CloseBookingReason.PaymentSuccess);
        Assert.Equal("some-id", input.bookingId);
        Assert.Equal(CloseBookingReason.PaymentSuccess, input.reason);
    }

    [Fact]
    public async Task Execute_InvalidGuid_ThrowsArgumentException()
    {
        var input = new CloseBookingInput("invalid-guid", CloseBookingReason.PaymentSuccess);

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _sut.Execute(input));

        Assert.StartsWith("Invalid booking ID format", exception.Message);
    }

    [Fact]
    public async Task Execute_BookingNotFound_ThrowsInvalidOperationException()
    {
        var bookingId = Guid.NewGuid();
        var input = new CloseBookingInput(bookingId.ToString(), CloseBookingReason.PaymentSuccess);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.Execute(input));

        Assert.Contains(bookingId.ToString(), exception.Message);
    }

    [Fact]
    public async Task Execute_PaymentSuccess_ClosesBookingAndSaves()
    {
        var movieEvent = MovieEvent.Create(
            new MovieId(Guid.NewGuid()),
            new RoomId(Guid.NewGuid()),
            DateTime.UtcNow.AddDays(1).Date.Add(new TimeSpan(19, 0, 0)),
            10
        );
        var booking = movieEvent.Book(2, 1);
        _uow.SetMovieEventByBookingId(booking.Id, movieEvent);

        await _sut.Execute(new CloseBookingInput(booking.Id.Value.ToString(), CloseBookingReason.PaymentSuccess));

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.True(_uow.SaveWasCalled);
        Assert.True(_uow.DoWasCalled);
    }

    [Fact]
    public async Task Execute_PaymentFailed_ClosesBookingReleasesSeatsAndSaves()
    {
        var movieEvent = MovieEvent.Create(
            new MovieId(Guid.NewGuid()),
            new RoomId(Guid.NewGuid()),
            DateTime.UtcNow.AddDays(1).Date.Add(new TimeSpan(19, 0, 0)),
            10
        );
        var booking = movieEvent.Book(2, 1);
        _uow.SetMovieEventByBookingId(booking.Id, movieEvent);

        await _sut.Execute(new CloseBookingInput(booking.Id.Value.ToString(), CloseBookingReason.PaymentFailed));

        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(0, movieEvent.Visitors);
        Assert.True(_uow.SaveWasCalled);
        Assert.True(_uow.DoWasCalled);
    }
}

public class StubUnitOfWorkForCloseBooking : IUnitOfWork
{
    private readonly Dictionary<MovieEventId, MovieEvent> _movieEventsByEventId = new();
    private readonly Dictionary<BookingId, MovieEvent> _movieEventsByBookingId = new();
    public bool SaveWasCalled { get; private set; }
    public bool DoWasCalled { get; private set; }

    public void SetMovieEventByBookingId(BookingId bookingId, MovieEvent movieEvent)
    {
        _movieEventsByBookingId[bookingId] = movieEvent;
    }

    public TRepository Repo<TRepository>() where TRepository : IRepository
    {
        if (typeof(TRepository) == typeof(IMovieEventRepository))
            return (TRepository)(IRepository)new StubMovieEventRepositoryWithBookingId(_movieEventsByEventId, _movieEventsByBookingId);

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

public class StubMovieEventRepositoryWithBookingId(
    Dictionary<MovieEventId, MovieEvent> movieEventsByEventId,
    Dictionary<BookingId, MovieEvent> movieEventsByBookingId
) : IMovieEventRepository
{
    public Task<Optional<MovieEvent>> ById(MovieEventId id)
    {
        movieEventsByEventId.TryGetValue(id, out var movieEvent);
        return Task.FromResult(Optional.Of(movieEvent));
    }

    public Task<bool> Exists(MovieEventId id)
        => Task.FromResult(movieEventsByEventId.ContainsKey(id));

    public Task Save(MovieEvent aggregateRoot) => Task.CompletedTask;

    public Task Remove(MovieEvent aggregateRoot) => Task.CompletedTask;

    public Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
        => Task.FromResult<MovieEvent?>(null);

    public Task<Optional<MovieEvent>> GetByBookingId(BookingId bookingId)
    {
        movieEventsByBookingId.TryGetValue(bookingId, out var movieEvent);
        return Task.FromResult(Optional.Of(movieEvent));
    }
}
