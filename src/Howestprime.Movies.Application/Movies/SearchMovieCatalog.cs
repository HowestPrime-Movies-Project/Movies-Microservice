using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record SearchMovieCatalogInput(
    string? Title,
    string? Genres,
    string xUserRole
);

public class SearchMovieCatalog(
    ISearchMovieCatalogQuery searchMovieCatalogQuery,
    IAuthorizationService authorizationService,
    ILogger<SearchMovieCatalog> logger
    ) : IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>>
{
    public Task<IReadOnlyList<MovieData>> Execute(SearchMovieCatalogInput input)
    {
        List<string> normalizedGenres = NormalizeGenres(input.Genres);
        
        authorizationService.Authorize(input.xUserRole, nameof(SearchMovieCatalog));
        
        logger.LogInformation("Searching movie catalog with title '{Title}' and genres [{Genres}].", input.Title, string.Join(", ", normalizedGenres));
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