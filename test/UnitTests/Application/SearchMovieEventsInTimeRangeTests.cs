using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application;

public sealed class SearchMovieEventsInTimeRangeTests
{
    [Fact]
    public async Task Execute_WhenDatesAreMissing_ShouldDefaultBothToNow()
    {
        var query = new FakeSearchMovieEventsInTimeRangeQuery(Array.Empty<MovieEventData>());
        var logger = new TestLogger<SearchMovieEventsInTimeRange>();
        var useCase = new SearchMovieEventsInTimeRange(query, logger);

        DateTime before = DateTime.Now.ToUniversalTime();

        List<MovieEventData> result = await useCase.Execute(new SearchMovieEventsInTimeRangeInput());

        DateTime after = DateTime.Now.ToUniversalTime();

        Assert.Empty(result);
        Assert.NotNull(query.CapturedFilter);

        var (fromDate, toDate) = ExtractRange(query.CapturedFilter!);

        Assert.InRange(fromDate, before, after.AddSeconds(1));
        Assert.InRange(toDate, before, after.AddSeconds(1));
        Assert.True(fromDate <= toDate);
    }

    [Fact]
    public async Task Execute_WhenFromDateIsAfterToDate_ShouldThrow()
    {
        var query = new FakeSearchMovieEventsInTimeRangeQuery(Array.Empty<MovieEventData>());
        var logger = new TestLogger<SearchMovieEventsInTimeRange>();
        var useCase = new SearchMovieEventsInTimeRange(query, logger);

        var input = new SearchMovieEventsInTimeRangeInput(
            FromDate: DateTime.Now.AddDays(1),
            ToDate: DateTime.Now
        );

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithValidRange_ShouldReturnWrappedQueryResult()
    {
        var expectedMovies = new List<MovieEventData>
        {
            new MovieEventData
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ShowTime = DateTime.UtcNow,
                Capacity = 120,
                Room = new RoomData(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Main Hall", 150),
                Movie = new MovieData
                {
                    Id = Guid.NewGuid(),
                    Title = "Inception",
                    Description = "A mind-bending thriller",
                    ReleaseYear = 2010,
                    Duration = 148,
                    Genres = new List<GenreData> { new("sci-fi") },
                    Actors = new List<ActorData> { new("Leonardo DiCaprio") },
                    AgeRating = 16,
                    PosterUrl = "https://example.com/inception.jpg"
                }
            }
        };

        var query = new FakeSearchMovieEventsInTimeRangeQuery(expectedMovies);
        var logger = new TestLogger<SearchMovieEventsInTimeRange>();
        var useCase = new SearchMovieEventsInTimeRange(query, logger);

        var input = new SearchMovieEventsInTimeRangeInput(
            FromDate: DateTime.Now.AddDays(-1),
            ToDate: DateTime.Now.AddDays(1)
        );

        List<MovieEventData> result = await useCase.Execute(input);

        Assert.Same(expectedMovies, result);
        Assert.NotNull(query.CapturedFilter);

        var eventData = result.Single();
        Assert.Equal(Guid.Parse("33333333-3333-3333-3333-333333333333"), eventData.Id);
        Assert.Equal(expectedMovies[0].ShowTime, eventData.ShowTime);
        Assert.Equal(120, eventData.Capacity);
        Assert.Equal(Guid.Parse("22222222-2222-2222-2222-222222222222"), eventData.Room.Id);
        Assert.Equal("Main Hall", eventData.Room.Name);
        Assert.Equal(150, eventData.Room.Capacity);
        Assert.Equal("Inception", eventData.Movie.Title);
        Assert.Equal(2010, eventData.Movie.ReleaseYear);

        var copiedEvent = eventData with { Capacity = 121 };
        Assert.Equal(121, copiedEvent.Capacity);
        Assert.NotEqual(eventData, copiedEvent);

        var output = new SearchMovieEventsInTimeRangeOutput(result);
        Assert.Same(result, output.Movies);
    }

    private static (DateTime FromDate, DateTime ToDate) ExtractRange(Expression<Func<MovieEventData, bool>> filter)
    {
        if (filter.Body is not BinaryExpression { NodeType: ExpressionType.AndAlso } andAlso)
        {
            throw new InvalidOperationException("Unexpected filter shape.");
        }

        return (ExtractDateTime(andAlso.Left), ExtractDateTime(andAlso.Right));
    }

    private static DateTime ExtractDateTime(Expression expression)
    {
        Expression candidate = expression switch
        {
            BinaryExpression binary => binary.Right,
            UnaryExpression unary => unary.Operand,
            _ => expression
        };

        return Expression.Lambda<Func<DateTime>>(candidate).Compile().Invoke();
    }

    private sealed class FakeSearchMovieEventsInTimeRangeQuery(IReadOnlyList<MovieEventData> result) : ISearchMovieEventsInTimeRangeQuery
    {
        public Expression<Func<MovieEventData, bool>>? CapturedFilter { get; private set; }

        public Task<List<MovieEventData>> Fetch(Expression<Func<MovieEventData, bool>> movieEventFilter)
        {
            CapturedFilter = movieEventFilter;
            return Task.FromResult(result as List<MovieEventData> ?? result.ToList());
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


