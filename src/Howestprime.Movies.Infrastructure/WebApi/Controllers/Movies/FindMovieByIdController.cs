using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public static class FindMovieByIdController
{
    public static async Task<Results<Ok<Movie>, NotFound<string>, BadRequest<string>>> Invoke(
        [FromRoute] string id,
        [FromHeader(Name = "x-user-role"), Required] string xUserRole,
        [FromServices] IUseCase<FindMovieByIdInput, MovieData?> useCase
    )
    {
        FindMovieByIdInput input = new(
            id,
            xUserRole
        );
        MovieData? output = await useCase.Execute(input);
        
        if (output == null)
        {
            return TypedResults.NotFound($"No movie found with id {id}");
        }
        
        var movie = new Movie(
            output.Id,
            output.Title,
            output.Description,
            output.ReleaseYear,
            output.Duration,
            output.Genres.Select(g => g.Value).ToList(),
            output.Actors.Select(a => a.Value).ToList(),
            output.AgeRating,
            output.PosterUrl
        );
        
        return TypedResults.Ok(movie);
    }
}