using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;

namespace UnitTests.Application;

public sealed class SearchMovieCatalogTests
{
    [Fact]
    public async Task Execute_WithBlankGenres_ShouldAuthorizeFetchAndReturnQueryResult()
    {
        var expectedMovies = new List<MovieData>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Inception",
                Description = "A mind-bending thriller",
                ReleaseYear = 2010,
                Duration = 148,
                Genres = new List<GenreData> { new("sci-fi") },
                Actors = new List<ActorData> { new("Leonardo DiCaprio") },
                AgeRating = 16,
                PosterUrl = "https://example.com/inception.jpg"
            }
        };

        var query = new FakeSearchMovieCatalogQuery(expectedMovies);
        var authorization = new FakeAuthorizationService();
        var useCase = new SearchMovieCatalog(query, authorization);

        var input = new SearchMovieCatalogInput(
            Title: "Inception",
            Genres: "   ",
            xUserRole: "admin"
        );

        var result = await useCase.Execute(input);

        Assert.Same(expectedMovies, result);
        Assert.Equal(("admin", nameof(SearchMovieCatalog)), authorization.Calls.Single());
        var capturedFilter = query.CapturedFilter;
        Assert.True(capturedFilter is not null);

        var predicate = capturedFilter.Compile();
        var matches = predicate(new MovieData
        {
            Title = "Inception",
            Description = "Any description",
            ReleaseYear = 2024,
            Duration = 100,
            Genres = null!,
            Actors = new List<ActorData>(),
            AgeRating = 12,
            PosterUrl = "https://example.com/movie.jpg"
        });

        Assert.True(matches);
    }

    [Fact]
    public void NormalizeGenres_WithCommaSeparatedValues_ShouldTrimLowercaseAndRemoveEmptyEntries()
    {
        var useCase = new SearchMovieCatalog(new FakeSearchMovieCatalogQuery(Array.Empty<MovieData>()), new FakeAuthorizationService());

        var genres = useCase.NormalizeGenres(" Sci-Fi, Thriller,,  Drama ");

        Assert.Equal(new[] { "sci-fi", "thriller", "drama" }, genres);
    }

    [Fact]
    public void Records_ShouldExposeTheirValues()
    {
        var input = new SearchMovieCatalogInput("Inception", "Sci-Fi", "user");
        var actor = new ActorData("Leonardo DiCaprio");
        var genre = new GenreData("sci-fi");

        var movie = new MovieData
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = input.Title!,
            Description = "A mind-bending thriller",
            ReleaseYear = 2010,
            Duration = 148,
            Genres = new List<GenreData> { genre },
            Actors = new List<ActorData> { actor },
            AgeRating = 16,
            PosterUrl = "https://example.com/inception.jpg"
        };

        var updatedMovie = movie with { Title = "Inception: Reloaded" };

        Assert.Equal("Inception", input.Title);
        Assert.Equal("Sci-Fi", input.Genres);
        Assert.Equal("user", input.xUserRole);
        Assert.Equal("Leonardo DiCaprio", actor.Value);
        Assert.Equal("sci-fi", genre.Value);
        Assert.Equal(Guid.Parse("22222222-2222-2222-2222-222222222222"), movie.Id);
        Assert.Equal("Inception", movie.Title);
        Assert.Equal("A mind-bending thriller", movie.Description);
        Assert.Equal(2010, movie.ReleaseYear);
        Assert.Equal(148, movie.Duration);
        Assert.Equal(new[] { genre }, movie.Genres);
        Assert.Equal(new[] { actor }, movie.Actors);
        Assert.Equal(16, movie.AgeRating);
        Assert.Equal("https://example.com/inception.jpg", movie.PosterUrl);
        Assert.Equal("Inception: Reloaded", updatedMovie.Title);
        Assert.NotEqual(movie, updatedMovie);
    }

    private sealed class FakeSearchMovieCatalogQuery(IReadOnlyList<MovieData> result) : ISearchMovieCatalogQuery
    {
        public Expression<Func<MovieData, bool>>? CapturedFilter { get; private set; }

        public Task<IReadOnlyList<MovieData>> Fetch(Expression<Func<MovieData, bool>> movieFilter)
        {
            CapturedFilter = movieFilter;
            return Task.FromResult(result);
        }
    }

    private sealed class FakeAuthorizationService : IAuthorizationService
    {
        public List<(string Role, string Permission)> Calls { get; } = new();

        public void Authorize(string userRole, string requestedPermission)
        {
            Calls.Add((userRole, requestedPermission));
        }
    }
}
