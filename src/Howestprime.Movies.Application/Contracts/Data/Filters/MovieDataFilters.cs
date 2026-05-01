using System.Linq.Expressions;

namespace Howestprime.Movies.Application.Contracts.Data.Filters;

public static class MovieDataFilters
{
    public static Expression<Func<MovieData, bool>> ByTitleAndGenres
    (
        string? titleContains,
        List<string>? genres
    )
    {
        var normalizedTitle = titleContains?.ToLower();
        var normalizedGenres = genres?.Select(g => g.ToLower()).ToList();

        return movie =>
            (string.IsNullOrWhiteSpace(normalizedTitle) || movie.Title.ToLower().Contains(normalizedTitle))
            &&
            (normalizedGenres == null || normalizedGenres.Count == 0 || (movie.Genres != null && movie.Genres.Any(movieGenre => normalizedGenres.Contains(movieGenre.Value.ToLower()))));
    }
    
    public static Expression<Func<MovieData, bool>> ById(string id)
    {
        return movie => movie.Id == Guid.Parse(id);
    }
}