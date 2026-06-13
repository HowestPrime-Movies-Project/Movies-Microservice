using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public sealed class ScheduleMovieEventTests
{
    [Fact]
    public async Task Execute_WithValidInput_AndNoExistingEvent_ShouldCreateAndSaveMovieEvent()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var showTime = DateTime.UtcNow.AddDays(1).Date.AddHours(15);
        
        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: movieId.ToString(),
            roomId: roomId.ToString(),
            showTime: showTime
        );

        // Act
        var output = await useCase.Execute(input);

        // Assert
        Assert.True(uow.SaveCalled);
        Assert.Equal(typeof(IMovieEventRepository), uow.SaveRepositoryType);
        Assert.IsType<MovieEvent>(uow.SavedAggregate);
        Assert.True(uow.DoCalled);
        Assert.True(Guid.TryParse(output.movieEventId, out _));
        Assert.Contains(logger.Messages, m => m.Contains("Movie event with ID"));
    }

    [Fact]
    public async Task Execute_WithInvalidMovieId_ShouldThrow()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: "not-a-guid",
            roomId: Guid.NewGuid().ToString(),
            showTime: DateTime.UtcNow.AddDays(1)
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithInvalidRoomId_ShouldThrow()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: Guid.NewGuid().ToString(),
            roomId: "not-a-guid",
            showTime: DateTime.UtcNow.AddDays(1)
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WhenMovieDoesNotExist_ShouldThrow()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        
        var uow = new FakeUnitOfWork();
        uow.MovieExists = false;
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: movieId.ToString(),
            roomId: roomId.ToString(),
            showTime: DateTime.UtcNow.AddDays(1)
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WhenRoomDoesNotExist_ShouldThrow()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        
        var uow = new FakeUnitOfWork();
        uow.MovieExists = true;
        uow.RoomExists = false;
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: movieId.ToString(),
            roomId: roomId.ToString(),
            showTime: DateTime.UtcNow.AddDays(1)
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WhenExistingEventExists_ShouldReassignMovieIdInPlace()
    {
        // Arrange
        var newMovieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var showTime = DateTime.UtcNow.AddDays(2).Date.AddHours(15);
        var originalShowTime = DateTime.UtcNow.AddDays(1).Date.AddHours(15);

        var existingEvent = MovieEvent.Create(new MovieId(Guid.NewGuid()), new RoomId(Guid.NewGuid()), originalShowTime, 100);
        var existingEventId = existingEvent.Id.Value;

        var uow = new FakeUnitOfWork();
        uow.MovieExists = true;
        uow.RoomExists = true;
        uow.ExistingEvent = existingEvent;

        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: newMovieId.ToString(),
            roomId: roomId.ToString(),
            showTime: showTime
        );

        // Act
        var output = await useCase.Execute(input);

        // Assert
        Assert.False(uow.RemoveCalled, "Remove must NOT be called - the event is updated in place");
        Assert.True(uow.SaveCalled);
        Assert.True(uow.DoCalled);
        Assert.Equal(existingEventId.ToString(), output.movieEventId);
        Assert.Equal(newMovieId, existingEvent.MovieId.Value);
    }

    [Fact]
    public async Task Execute_WithValidShowtimeAt19h_ShouldSucceed()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var showTime = DateTime.UtcNow.AddDays(1).Date.AddHours(19);

        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: movieId.ToString(),
            roomId: roomId.ToString(),
            showTime: showTime
        );

        // Act
        var output = await useCase.Execute(input);

        // Assert
        Assert.True(Guid.TryParse(output.movieEventId, out _));
    }

    [Fact]
    public async Task Execute_WithUnspecifiedShowtimeKind_ShouldTreatInputAsUtc()
    {
        // Arrange
        var movieId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var showTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(1).Date.AddHours(15), DateTimeKind.Unspecified);

        var uow = new FakeUnitOfWork();
        var logger = new TestLogger<ScheduleMovieEvent>();
        var useCase = new ScheduleMovieEvent(uow, logger);

        var input = new ScheduleMovieEventInput(
            movieId: movieId.ToString(),
            roomId: roomId.ToString(),
            showTime: showTime
        );

        // Act
        var output = await useCase.Execute(input);

        // Assert
        Assert.True(Guid.TryParse(output.movieEventId, out _));
        Assert.True(uow.SaveCalled);
        Assert.True(uow.DoCalled);
    }

    // Test doubles
    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public bool SaveCalled { get; private set; }
        public Type? SaveRepositoryType { get; private set; }
        public IAggregateRoot? SavedAggregate { get; private set; }
        public bool DoCalled { get; private set; }
        public bool RemoveCalled { get; set; }
        public bool MovieExists { get; set; } = true;
        public bool RoomExists { get; set; } = true;
        public MovieEvent? ExistingEvent { get; set; }

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
            return Task.CompletedTask;
        }

        public TRepository Repo<TRepository>() where TRepository : IRepository
        {
            return (TRepository)(object)new FakeRepository(this);
        }
    }

    private sealed class FakeRepository : IMovieRepository, IRoomRepository, IMovieEventRepository
    {
        private readonly FakeUnitOfWork _uow;

        public FakeRepository(FakeUnitOfWork uow)
        {
            _uow = uow;
        }

        Task<bool> IRepository<Movie, MovieId>.Exists(MovieId id) => Task.FromResult(_uow.MovieExists);
        Task<bool> IRepository<Room, RoomId>.Exists(RoomId id) => Task.FromResult(_uow.RoomExists);
        Task<bool> IRepository<MovieEvent, MovieEventId>.Exists(MovieEventId id) => Task.FromResult(false);

        Task<Aornis.Optional<Movie>> IRepository<Movie, MovieId>.ById(MovieId id) => throw new NotImplementedException();
        Task<Aornis.Optional<Room>> IRepository<Room, RoomId>.ById(RoomId id)
        {
            if (!_uow.RoomExists)
            {
                return Task.FromResult(Optional.Of<Room>(null));
            }

            Room room = Room.Create("Test room", 150, id);
            return Task.FromResult(Optional.Of(room));
        }
        Task<Aornis.Optional<MovieEvent>> IRepository<MovieEvent, MovieEventId>.ById(MovieEventId id) => throw new NotImplementedException();

        Task IRepository<Movie, MovieId>.Save(Movie aggregateRoot) => Task.CompletedTask;
        Task IRepository<Room, RoomId>.Save(Room aggregateRoot) => Task.CompletedTask;
        Task IRepository<MovieEvent, MovieEventId>.Save(MovieEvent aggregateRoot) => Task.CompletedTask;

        Task IRepository<Movie, MovieId>.Remove(Movie aggregateRoot) => Task.CompletedTask;
        Task IRepository<Room, RoomId>.Remove(Room aggregateRoot) => Task.CompletedTask;
        Task IRepository<MovieEvent, MovieEventId>.Remove(MovieEvent aggregateRoot)
        {
            _uow.RemoveCalled = true;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
        {
            return Task.FromResult(_uow.ExistingEvent != null);
        }

        public Task<MovieEvent?> GetByShowtimeAndRoomId(DateTime showTime, RoomId roomId)
        {
            return Task.FromResult(_uow.ExistingEvent);
        }

        public Task<Aornis.Optional<MovieEvent>> GetByBookingId(BookingId bookingId)
        {
            return Task.FromResult(Optional.Of<MovieEvent>(null));
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







