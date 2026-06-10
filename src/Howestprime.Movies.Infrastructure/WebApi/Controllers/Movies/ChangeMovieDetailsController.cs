using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record ChangeMovieDetailsRequest(
    [FromRoute] string id,
    [FromBody] ChangeMovieDetailsBody body,
    [FromServices] IUseCase<ChangeMovieDetailsInput> useCase
);

public record ChangeMovieDetailsBody(
    [Required] string title,
    [Required] string description,
    [Required] int releaseYear,
    [Required] int duration,
    [Required] List<string> genres,
    [Required] List<string> actors,
    [Required] int ageRating,
    [Required] string posterUrl
);

public static class ChangeMovieDetailsController
{
    public static async Task<Results<NoContent, BadRequest>> Invoke(
        [AsParameters] ChangeMovieDetailsRequest request
    )
    {
        ChangeMovieDetailsInput input = new(
            request.id,
            request.body.title,
            request.body.description,
            request.body.duration,
            request.body.genres,
            request.body.actors,
            request.body.releaseYear,
            request.body.ageRating,
            request.body.posterUrl
        );

        await request.useCase.Execute(input);

        return TypedResults.NoContent();
    }
}
