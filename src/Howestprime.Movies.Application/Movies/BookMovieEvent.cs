using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record BookMovieEventInput(
    string movieEventId,
    int standardVisitors,
    int discountVisitors
);
public sealed record BookMovieEventOutput(
    string bookingId
);

public class BookMovieEvent(
    IUnitOfWork uow,
    ILogger<BookMovieEvent> logger
) : IUseCase<BookMovieEventInput, BookMovieEventOutput>
{
    public async Task<BookMovieEventOutput> Execute(BookMovieEventInput input)
    {
        if (!Guid.TryParse(input.movieEventId, out Guid movieEventIdGuid)) throw new ArgumentException("Invalid movie event ID format.", nameof(input.movieEventId));
        MovieEventId movieEventId = new(movieEventIdGuid);

        IMovieEventRepository movieEventRepository = uow.Repo<IMovieEventRepository>();
        Optional<MovieEvent> movieEventOptional = await movieEventRepository.ById(movieEventId);

        if (!movieEventOptional.HasValue) throw new InvalidOperationException($"Movie event with ID {movieEventId.Value} not found.");
        MovieEvent movieEvent = movieEventOptional.Value;
        Booking booking = movieEvent.Book(input.standardVisitors, input.discountVisitors);
  
        await uow.Save<IMovieEventRepository>(movieEvent);
        await uow.Do();

        logger.LogInformation(
            "Booking {BookingId} created successfully for movie event {MovieEventId}.",
            booking.Id.Value,
            movieEvent.Id.Value);

        return new BookMovieEventOutput(booking.Id.ToString());

    }
}