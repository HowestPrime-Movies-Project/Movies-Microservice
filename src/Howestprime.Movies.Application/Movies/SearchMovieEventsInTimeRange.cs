using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies;

public sealed record SearchMovieEventsInTimeRangeInput(
    DateTime? FromDate = null,
    DateTime? ToDate = null
    );

public sealed record SearchMovieEventsInTimeRangeOutput(
    List<MovieEventData> Movies
    );

public class SearchMovieEventsInTimeRange(
    ISearchMovieEventsInTimeRangeQuery searchMovieEventsInTimeRangeQuery
    ) : IUseCase<SearchMovieEventsInTimeRangeInput, List<MovieEventData>>
{
    public async Task<List<MovieEventData>> Execute(SearchMovieEventsInTimeRangeInput input)
    {
        DateTime fromDate = (input.FromDate ?? DateTime.Now).ToUniversalTime();
        DateTime toDate = (input.ToDate ?? DateTime.Now).ToUniversalTime();

        ValidateTimeRange(fromDate, toDate);

        return await searchMovieEventsInTimeRangeQuery.Fetch(MovieEventDataFilters.ByTimeRange(fromDate, toDate));
    }

    private static void ValidateTimeRange(DateTime fromDate, DateTime toDate)
    {
        if (fromDate > toDate)
        {
            throw new ArgumentException("FromDate must be less than or equal to ToDate.", nameof(fromDate));
        }
    }
}