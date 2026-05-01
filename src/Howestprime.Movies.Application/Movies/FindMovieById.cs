using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies;

public sealed record FindMovieByIdInput(
    string Id,
    string xUserRole
);

public class FindMovieById(
    IFindMovieByIdQuery findMovieByIdQuery,
    IAuthorizationService authorizationService
    ) : IUseCase<FindMovieByIdInput, MovieData?>
{
    public Task<MovieData?> Execute(FindMovieByIdInput input)
    {
        authorizationService.Authorize(input.xUserRole, nameof(FindMovieById));
        
        return findMovieByIdQuery.Fetch(MovieDataFilters.ById(input.Id));
    }
}