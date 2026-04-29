using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public record Genre : ValueObject
{
    public string Value { get; init; }
}