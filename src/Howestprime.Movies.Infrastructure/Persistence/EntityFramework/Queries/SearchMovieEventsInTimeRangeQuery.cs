using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Queries;

public class SearchMovieEventsInTimeRangeQuery(QueryDbContext ctx) : ISearchMovieEventsInTimeRangeQuery
{
    public Task<List<MovieEventData>> Fetch(Expression<Func<MovieEventData, bool>> movieEventFilter)
    {
        return ctx.MovieEvents
            .Where(movieEventFilter)
            .ToListAsync();
    }
}