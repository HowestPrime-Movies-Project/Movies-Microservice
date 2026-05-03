using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public static class SearchMovieEventsInTimeRangeController
{
    public static async Task<Results<Ok<HowestprimeSchedule>, BadRequest<string>>> Invoke(
        [FromServices] IUseCase<SearchMovieEventsInTimeRangeInput, List<MovieEventData>> useCase
    )
    {
        SearchMovieEventsInTimeRangeInput input = new(
            DateTime.Now,
            DateTime.Now.AddDays(14)
        );

        List<MovieEventData> output = await useCase.Execute(input);

        var schedule = ConvertToHowestprimeSchedule(output);

        return TypedResults.Ok(schedule);
    }

    private static HowestprimeSchedule ConvertToHowestprimeSchedule(List<MovieEventData> output)
    {
        var movieEvents = output.Select(m => new MovieEvent(
            m.Id,
            m.ShowTime,
            m.Capacity,
            new Room(m.Room.Id, m.Room.Name, m.Room.Capacity),
            new Movie(
                m.Movie.Id,
                m.Movie.Title,
                m.Movie.Description,
                m.Movie.ReleaseYear,
                m.Movie.Duration,
                m.Movie.Genres.Select(g => g.Value).ToList(),
                m.Movie.Actors.Select(a => a.Value).ToList(),
                m.Movie.AgeRating,
                m.Movie.PosterUrl
            )
        )).ToList();

        return new HowestprimeSchedule(
            output.Select(m => m.Movie.Id).Distinct().ToList(),
            movieEvents
        );
    }
}

