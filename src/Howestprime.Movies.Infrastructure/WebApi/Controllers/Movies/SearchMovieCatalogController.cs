using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
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
    public static async Task<Results<Ok<IReadOnlyList<MovieData>>, BadRequest<string>>> Invoke(
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
        
        return TypedResults.Ok(output);
    }
    
}