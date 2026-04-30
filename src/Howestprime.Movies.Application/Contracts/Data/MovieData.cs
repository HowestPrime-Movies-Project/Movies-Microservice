namespace Howestprime.Movies.Application.Contracts.Data
{
    public sealed record MovieData
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int ReleaseYear { get; init; }
        public int Duration { get; init; }
        public IList<GenreData> Genres { get; init; }
        public IList<ActorData> Actors { get; init; }
        public int AgeRating { get; init; }
        public string PosterUrl { get; init; }
    }
}

