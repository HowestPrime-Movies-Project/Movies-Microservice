namespace Howestprime.Movies.Application.Contracts.Data
{
    public sealed record MovieData
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public int ReleaseYear { get; init; }
        public int Duration { get; init; }
        public required IList<GenreData> Genres { get; init; }
        public required IList<ActorData> Actors { get; init; }
        public int AgeRating { get; init; }
        public required string PosterUrl { get; init; }
    }
}

