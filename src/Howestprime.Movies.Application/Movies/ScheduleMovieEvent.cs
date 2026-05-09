using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record ScheduleMovieEventInput(
    string movieId,
    string roomId,
    DateTime showTime
);

public sealed record ScheduleMovieEventOutput(
    string movieEventId
);

public sealed class ScheduleMovieEvent(
    IUnitOfWork uow,
    ILogger<ScheduleMovieEvent> logger
    ) : IUseCase<ScheduleMovieEventInput, ScheduleMovieEventOutput>
{
    public async Task<ScheduleMovieEventOutput> Execute(ScheduleMovieEventInput input)
    {
        
        DateTime universalTime = input.showTime.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(input.showTime, DateTimeKind.Utc) : input.showTime.ToUniversalTime();
 
        // Validate input
        if (!Guid.TryParse(input.movieId, out Guid movieIdGuid))
            throw new ArgumentException("Invalid movie ID format.", nameof(input.movieId));

        if (!Guid.TryParse(input.roomId, out Guid roomIdGuid))
            throw new ArgumentException("Invalid room ID format.", nameof(input.roomId));

        MovieId movieId = new(movieIdGuid);
        RoomId roomId = new(roomIdGuid);

        // Check if movie exists
        IMovieRepository movieRepository = uow.Repo<IMovieRepository>();
        bool movieExists = await movieRepository.Exists(movieId);
        if (!movieExists)
            throw new InvalidOperationException($"Movie with ID {movieId.Value} not found.");

        // Check if room exists
        IRoomRepository roomRepository = uow.Repo<IRoomRepository>();
        Optional<Room> roomOptional = await roomRepository.ById(roomId);
        if (!roomOptional.HasValue)
            throw new InvalidOperationException($"Room with ID {roomId.Value} not found.");

        // Check if showtime and room are already booked
        IMovieEventRepository movieEventRepository = uow.Repo<IMovieEventRepository>();
        var existingEvent = await movieEventRepository.GetByShowtimeAndRoomId(universalTime, roomId);
        
        MovieEvent movieEvent;
        
        if (existingEvent != null)
        {
            // Overwrite: Remove the old event and create a new one
            await movieEventRepository.Remove(existingEvent);
            movieEvent = MovieEvent.Create(movieId, roomId, universalTime, roomOptional.Value.Capacity);
        }
        else
        {
            // Create new movie event
            movieEvent = MovieEvent.Create(movieId, roomId, universalTime, roomOptional.Value.Capacity);
        }
        
        // Save the movie event
        await uow.Save<IMovieEventRepository>(movieEvent);
        await uow.Do();

        logger.LogInformation("Movie event with ID {MovieEventId} scheduled successfully for movie {MovieId} in room {RoomId} at {ShowTime}.", 
            movieEvent.Id.Value, movieId.Value, roomId.Value, universalTime);

        return new ScheduleMovieEventOutput(movieEvent.Id.Value.ToString());
    }
}




