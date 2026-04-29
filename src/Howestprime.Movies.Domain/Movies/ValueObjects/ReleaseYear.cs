using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed record ReleaseYear : ValueObject
{
    public int Year { get; init; }
    
    public static ReleaseYear From(int year) => new() { Year = year };

}