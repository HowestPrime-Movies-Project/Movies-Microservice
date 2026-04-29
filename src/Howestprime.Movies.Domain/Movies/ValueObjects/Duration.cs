using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies;

public record Duration : ValueObject
{
    public int Runtime { get; init; }
    
    public static Duration From(int duration) => new() { Runtime = duration };

}