
using System.Linq.Expressions;
using Aornis;
using Howestprime.Movies.Application.Contracts.Data;

namespace Howestprime.Movies.Application.Contracts.Ports;

public interface ISearchMovieCatalogQuery
{
    public Task<IReadOnlyList<MovieData>> Fetch(
        Expression<Func<MovieData, bool>> movieFilter
    );
}

public interface IFindMovieByIdQuery
{
    public Task<MovieData?> Fetch(
        Expression<Func<MovieData, bool>> movieFilter
    );
}