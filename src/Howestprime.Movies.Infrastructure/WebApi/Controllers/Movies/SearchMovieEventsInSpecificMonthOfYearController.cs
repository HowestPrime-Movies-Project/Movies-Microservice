using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public static class SearchMovieEventsInSpecificMonthOfYearController
{
    public static async Task<Results<Ok<HowestprimeSchedule>, BadRequest<string>>> Invoke(
        [FromQuery, Required] string? Month,
        [FromQuery, Required] string? Year,
        [FromServices] IUseCase<SearchMovieEventsInTimeRangeInput, List<MovieEventData>> useCase
    )
    {
        if (Month is null || Year is null) throw new ArgumentException("Month and Year are required");
        int month = int.Parse(Month);
        int year = int.Parse(Year);
        
        DateTime startDate = new DateTime(year, month, 1, 0, 0, 1, DateTimeKind.Utc);
        DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, DateTimeKind.Utc);

        SearchMovieEventsInTimeRangeInput input = new(
            startDate,
            endDate
        );

        List<MovieEventData> output = await useCase.Execute(input);

        var schedule = ConvertToHowestprimeSchedule(output);

        return TypedResults.Ok(schedule);
    }
    
    private static HowestprimeSchedule ConvertToHowestprimeSchedule(List<MovieEventData> output)
    {
        if (output.Count == 0) return new HowestprimeSchedule(new List<Guid>(), new List<MovieEvent>());
        
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