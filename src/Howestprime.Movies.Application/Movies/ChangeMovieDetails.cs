using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record ChangeMovieDetailsInput(
    string movieId,
    string title,
    string description,
    int duration,
    List<string> genres,
    List<string> actors,
    int releaseYear,
    int ageRating,
    string posterUrl
);

public class ChangeMovieDetails(
    IUnitOfWork uow,
    ILogger<ChangeMovieDetails> logger
) : IUseCase<ChangeMovieDetailsInput>
{
    public async Task Execute(ChangeMovieDetailsInput input)
    {
        if (!Guid.TryParse(input.movieId, out Guid movieIdGuid)) throw new ArgumentException("Invalid movie ID format.", nameof(input.movieId));
        MovieId movieId = new(movieIdGuid);

        IMovieRepository movieRepository = uow.Repo<IMovieRepository>();
        var movieOptional = await movieRepository.ById(movieId);

        if (!movieOptional.HasValue) throw new InvalidOperationException($"Movie with ID {movieId.Value} not found.");
        Movie movie = movieOptional.Value;

        movie.ChangeDetails(
            input.title,
            input.description,
            new ReleaseYear { Year = input.releaseYear },
            new Duration { Runtime = input.duration },
            input.genres.Select(g => new Genre { Value = g }).ToList(),
            input.actors.Select(a => new Actor { Value = a }).ToList(),
            new AgeRating { Age = input.ageRating },
            new PosterUrl { Url = input.posterUrl }
        );

        await uow.Save<IMovieRepository>(movie);
        await uow.Do();

        logger.LogInformation("Movie with ID {MovieId} details changed successfully.", movie.Id.Value);
    }
}
