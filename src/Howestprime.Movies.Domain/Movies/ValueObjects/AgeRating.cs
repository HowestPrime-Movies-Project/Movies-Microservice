using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public record AgeRating : ValueObject
{
    public int Age { get; init; }

    public static AgeRating From(int age) => new() { Age = age };
}