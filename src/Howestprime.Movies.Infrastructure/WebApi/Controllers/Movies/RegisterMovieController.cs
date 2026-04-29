using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record RegisterMovieRequest(
    [FromBody] RegisterMovieBody body,
    [FromServices] IUseCase<RegisterMovieInput, RegisterMovieOutput> useCase);

public record RegisterMovieBody(
    [Required] string title,
    [Required] string description,
    [Required] int releaseYear,
    [Required] int duration,
    [Required] List<string> genres,
    [Required] List<string> actors,
    [Required] int ageRating,
    [Required] string posterUrl
);
    

public static class RegisterMovieController
{
    public static async Task<Results<Created<string>, BadRequest>> Invoke(
        [AsParameters] RegisterMovieRequest request
    )
    {
        RegisterMovieInput input = new(
            request.body.title,
            request.body.description,
            request.body.duration,
            request.body.genres,
            request.body.actors,
            request.body.releaseYear,
            request.body.ageRating,
            request.body.posterUrl
        );

        RegisterMovieOutput output = await request.useCase.Execute(input);

        return TypedResults.Created($"/api/movie-catalog/{output.movieId}", output.movieId);
    }
}


