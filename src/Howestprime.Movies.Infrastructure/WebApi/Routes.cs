using Howestprime.Movies.Infrastructure.WebApi.Controllers;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Howestprime.Movies.Infrastructure.WebApi;

public static class Routes
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder webApi = app.MapGroup("/api");

        webApi.MapMovieRoutes();
        webApi.MapMovieEventRoutes();
        webApi.MapHowestPrimeScheduleRoutes();

        return app;
    }
    
    private static RouteGroupBuilder MapMovieRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder movies = app.MapGroup("/movie-catalog")
            .WithTags("Movies")
            .WithDescription("All endpoints related to managing the movie catalog.");
        
        movies.MapPost("/", RegisterMovieController.Invoke)
            .WithName("RegisterMovie")
            .WithDescription("Register a new movie.");
        
        movies.MapGet("/", SearchMovieCatalogController.Invoke)
            .WithName("SearchMovieCatalog")
            .WithDescription("Search the movie catalog by title and genres.");

        movies.MapGet("/{movieId}", FindMovieByIdController.Invoke)
            .WithName("FindMovieById")
            .WithDescription("Find a movie by its id.");
        return movies;
    }
    
    private static RouteGroupBuilder MapMovieEventRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder movieEvents = app.MapGroup("/movie-events")
            .WithTags("Movie Events")
            .WithDescription("All endpoints related to managing movie events.");
        
        movieEvents.MapPost("/", ScheduleMovieEventController.Invoke)
            .WithName("ScheduleMovieEvent")
            .WithDescription("Schedule a new movie event.");
        
        movieEvents.MapGet("/", SearchMovieEventsInSpecificMonthOfYearController.Invoke)
            .WithName("SearchMovieEventsInSpecificMonthOfYear")
            .WithDescription("Get The movie schedule for a specific month of a specific year.");

        movieEvents.MapPost("/{movieEventId}/bookings", BookMovieEventController.Invoke)
            .WithName("BookMovieEvent")
            .WithDescription("Book a movie event by its id.");
        
        return movieEvents;
    }
    
    private static RouteGroupBuilder MapHowestPrimeScheduleRoutes(this RouteGroupBuilder app)
    {
        RouteGroupBuilder movieEvents = app.MapGroup("/howestprime-schedule")
            .WithTags("Howestprime Schedule")
            .WithDescription("Search the scheduled movie events.");
        
        movieEvents.MapGet("/", SearchMovieEventsInTimeRangeController.Invoke)
            .WithName("SearchMovieEventsInTimeRange")
            .WithDescription("Get The movie schedule for the next 14 days.");
        

        return movieEvents;
    }
}

