using System.Linq.Expressions;
using Aornis;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Queries;

public class MovieByTitleAndGenresQuery(
    QueryDbContext ctx
    ) : ISearchMovieCatalogQuery
{
    public async Task<IReadOnlyList<MovieData>> Fetch(Expression<Func<MovieData, bool>> movieFilter)
    {
        IReadOnlyList<MovieData>? movies = await ctx.Movies
            .Where(movieFilter)
            .ToListAsync();

        return movies;
    }
}