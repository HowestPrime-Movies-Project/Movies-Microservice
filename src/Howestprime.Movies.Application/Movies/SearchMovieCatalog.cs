using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies;

public sealed record SearchMovieCatalogInput(
    string? Title,
    string? Genres,
    string xUserRole
);

public class SearchMovieCatalog(
    ISearchMovieCatalogQuery searchMovieCatalogQuery,
    IAuthorizationService authorizationService
    ) : IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>>
{
    public Task<IReadOnlyList<MovieData>> Execute(SearchMovieCatalogInput input)
    {
        List<string> normalizedGenres = NormalizeGenres(input.Genres);
        
        authorizationService.Authorize(input.xUserRole, nameof(SearchMovieCatalog));
        
        return searchMovieCatalogQuery.Fetch(MovieDataFilters.ByTitleAndGenres(input.Title, normalizedGenres));
    }
    
    public List<string> NormalizeGenres(string? genres)
    {
        if (string.IsNullOrWhiteSpace(genres))
        {
            return [];
        }

        return genres.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(g => g.ToLowerInvariant())
            .ToList();
    }
}