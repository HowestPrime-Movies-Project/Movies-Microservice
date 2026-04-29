using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public record PosterUrl : ValueObject
{
    public string Url { get; init; }
    
    public static PosterUrl From(string url) => new() { Url = url };

}