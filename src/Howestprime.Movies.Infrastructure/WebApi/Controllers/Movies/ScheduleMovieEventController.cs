using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record ScheduleMovieEventRequest(
    [FromBody] ScheduleMovieEventBody body,
    [FromServices] IUseCase<ScheduleMovieEventInput, ScheduleMovieEventOutput> useCase);

public record ScheduleMovieEventBody(
    [Required] string movieId,
    [Required] string roomId,
    [Required] DateTime showTime,
    [Required] int capacity
);

public static class ScheduleMovieEventController
{
    public static async Task<Results<Created<string>, BadRequest>> Invoke(
        [AsParameters] ScheduleMovieEventRequest request
    )
    {
        ScheduleMovieEventInput input = new(
            request.body.movieId,
            request.body.roomId,
            request.body.showTime,
            request.body.capacity
        );

        ScheduleMovieEventOutput output = await request.useCase.Execute(input);

        return TypedResults.Created($"/api/movie-events/{output.movieEventId}", output.movieEventId);
    }
}

