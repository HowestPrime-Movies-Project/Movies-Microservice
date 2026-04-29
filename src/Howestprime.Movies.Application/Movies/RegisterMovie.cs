using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record RegisterMovieInput(
    string title,
    string description,
    int duration,
    List<string> genres,
    List<string> actors,
    int releaseYear,
    int ageRating,
    string posterUrl
);

public sealed record RegisterMovieOutput(
    string movieId
);

public sealed class RegisterMovie(
    IUnitOfWork uow,
    ILogger<RegisterMovie> logger
    ) : IUseCase<RegisterMovieInput, RegisterMovieOutput>
{
    public async Task<RegisterMovieOutput> Execute(RegisterMovieInput input)
    {
        Movie movie = Movie.Create(
            input.title,
            input.description,
            new ReleaseYear { Year = input.releaseYear },
            new Duration {Runtime = input.duration},
            input.genres.Select(g => new Genre {Value = g}).ToList(),
            input.actors.Select(a => new Actor {Value = a}).ToList(),
            new AgeRating {Age = input.ageRating},
            new PosterUrl {Url = input.posterUrl}
        );
        
        await uow.Save<IMovieRepository>(movie);

        await uow.Do();
        
        logger.LogInformation("Movie with ID {MovieId} registered successfully.", movie.Id.Value);

        return new RegisterMovieOutput(movie.Id.Value.ToString());
    }
}