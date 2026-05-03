using System.Linq.Expressions;

namespace Howestprime.Movies.Application.Contracts.Data.Filters;

public class MovieEventDataFilters
{
    public static Expression<Func<MovieEventData, bool>> ByTimeRange(DateTime fromDate, DateTime toDate)
    {
        return movieEvent =>
            movieEvent.ShowTime >= fromDate && movieEvent.ShowTime <= toDate;
    }
}