using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Queries;

public class MovieByIdQuery(
    QueryDbContext ctx
    ) : IFindMovieByIdQuery
{
    public Task<MovieData?> Fetch(Expression<Func<MovieData, bool>> movieFilter)
    {
        return ctx.Movies
            .Where(movieFilter)
            .FirstOrDefaultAsync();
    }
}