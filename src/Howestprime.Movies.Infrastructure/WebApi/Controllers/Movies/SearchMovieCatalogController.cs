using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record SearchMovieCatalogRequest(
    [FromQuery] string? title,
    [FromQuery] string? genres,
    [FromServices] IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>> useCase
    );

public static class SearchMovieCatalogController
{
    public static async Task<Results<Ok<MovieCollection>, BadRequest<string>>> Invoke(
        [AsParameters] SearchMovieCatalogRequest request,
        [FromHeader(Name = "x-user-role"), Required] string xUserRole
    )
    {

        SearchMovieCatalogInput input = new(
            request.title,
            request.genres,
            xUserRole
        );
        IReadOnlyList<MovieData> output = await request.useCase.Execute(input);
        
        var movies = output.Select(m => new Movie(
            m.Id,
            m.Title,
            m.Description,
            m.ReleaseYear,
            m.Duration,
            m.Genres.Select(g => g.Value).ToList(),
            m.Actors.Select(a => a.Value).ToList(),
            m.AgeRating,
            m.PosterUrl
        )).ToList();
        
        return TypedResults.Ok(new MovieCollection(movies));
    }
    
}