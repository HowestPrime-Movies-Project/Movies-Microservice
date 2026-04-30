using System.Linq.Expressions;

namespace Howestprime.Movies.Application.Contracts.Data.Filters;

public static class MovieDataFilters
{
    public static Expression<Func<MovieData, bool>> ByTitleAndGenres
    (
        string? titleContains, 
        IList<GenreData>? genres
    ) 
    {
        return movie => 
            (string.IsNullOrWhiteSpace(titleContains) || movie.Title.Contains(titleContains)) &&
            (genres == null || genres.Count == 0 || (movie.Genres != null && genres.All(genre => movie.Genres.Contains(genre))));
    }
}