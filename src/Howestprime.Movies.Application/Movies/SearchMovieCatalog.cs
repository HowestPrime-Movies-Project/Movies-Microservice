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
        IList<GenreData> normalizedGenres = NormalizeGenres(input.Genres);
        
        authorizationService.Authorize(input.xUserRole, nameof(SearchMovieCatalog));
        
        return searchMovieCatalogQuery.Fetch(MovieDataFilters.ByTitleAndGenres(input.Title, normalizedGenres));
    }
    
    public IList<GenreData> NormalizeGenres(string? genres)
    {
        if (string.IsNullOrWhiteSpace(genres))
        {
            return [];
        }

        return genres.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(g => new GenreData(g.ToLowerInvariant()))
            .ToList();
    }
}