using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record CloseBookingInput(
    string bookingId,
    CloseBookingReason reason
);

public class CloseBooking(
    IUnitOfWork uow,
    ILogger<CloseBooking> logger
) : IUseCase<CloseBookingInput>
{
    public async Task Execute(CloseBookingInput input)
    {
        if (!Guid.TryParse(input.bookingId, out Guid bookingIdGuid)) throw new ArgumentException("Invalid booking ID format.", nameof(input.bookingId));
        BookingId bookingId = new(bookingIdGuid);

        IMovieEventRepository movieEventRepository = uow.Repo<IMovieEventRepository>();
        var movieEventOptional = await movieEventRepository.GetByBookingId(bookingId);

        if (!movieEventOptional.HasValue) throw new InvalidOperationException($"Movie event with booking ID {bookingId.Value} not found.");
        MovieEvent movieEvent = movieEventOptional.Value;

        movieEvent.CloseBooking(bookingId, input.reason);

        await uow.Save<IMovieEventRepository>(movieEvent);
        await uow.Do();

        logger.LogInformation(
            "Booking {BookingId} closed with reason {Reason}.",
            bookingId.Value,
            input.reason);
    }
}
