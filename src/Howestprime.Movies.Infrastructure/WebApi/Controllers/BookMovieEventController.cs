using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

public record BookMovieEventRequest(
    [FromRoute] string movieEventId,
    [FromBody] BookMovieEventBody body,
    [FromServices] IUseCase<BookMovieEventInput, BookMovieEventOutput> useCase
    );

public record BookMovieEventBody(
    [Required] int standardVisitors,
    [Required] int discountVisitors
);

public static class BookMovieEventController
{
    public static async Task<Results<Created<string>, BadRequest>> Invoke(
        [AsParameters] BookMovieEventRequest request
    )
    {
        BookMovieEventInput input = new(
            request.movieEventId,
            request.body.standardVisitors,
            request.body.discountVisitors
        );
        
        BookMovieEventOutput output = await request.useCase.Execute(input);
        
        return TypedResults.Created($"/api/movie-events/{request.movieEventId}/bookings/{output.bookingId}", output.bookingId);
        
    }
}