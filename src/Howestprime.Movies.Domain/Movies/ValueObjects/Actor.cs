using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public sealed record Actor : ValueObject
{
    public string Value { get; init; }
}