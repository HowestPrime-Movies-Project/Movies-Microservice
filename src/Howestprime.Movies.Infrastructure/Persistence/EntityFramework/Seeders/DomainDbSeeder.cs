using Microsoft.Extensions.Logging;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Seeders;

public class DomainDbSeeder(DomainDbContext context, ILogger<DomainDbSeeder> _logger)
{
    public async Task Seed()
    {
        await SeedRooms();
        await SeedMovies();
    }
    
    private async Task SeedRooms()
    {
        // Check if rooms already exist
        if (context.Rooms.Any())
            return;

        var rooms = new List<Room>
        {
            Room.Create("Blue Room", 100, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-71db-8a1a-ec7569492999"))),
            Room.Create("Yellow Room", 80, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-75fe-b936-0a97cd75216e")))
        };

        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {RoomCount} rooms.", rooms.Count);
    }    
    
    private async Task SeedMovies()
    {
        // Check if movies already exist
        if (context.Movies.Any())
            return;

        var movies = new List<Movie>
        {
            CreateMovie(
                "Marty Supreme",
                "An intense sports drama about an ambitious table tennis prodigy.",
                2026,
                140,
                13,
                "https://example.com/posters/marty-supreme.jpg",
                ["Drama", "Sports"],
                ["Timothée Chalamet"]
            ),
            CreateMovie(
                "Mickey 17",
                "A disposable worker is sent on a dangerous colony mission.",
                2025,
                137,
                13,
                "https://example.com/posters/mickey-17.jpg",
                ["Sci-Fi", "Adventure"],
                ["Robert Pattinson"]
            ),
            CreateMovie(
                "Novocaine",
                "A dark action thriller about a man who cannot feel pain.",
                2025,
                110,
                16,
                "https://example.com/posters/novocaine.jpg",
                ["Action", "Thriller"],
                ["Jack Quaid"]
            ),
            CreateMovie(
                "Kill Bill: Vol. 1",
                "A former assassin hunts down the gang that betrayed her.",
                2003,
                111,
                16,
                "https://example.com/posters/kill-bill-vol-1.jpg",
                ["Action", "Thriller"],
                ["Uma Thurman"]
            ),
            CreateMovie(
                "The Blair Witch Project",
                "A found-footage horror story about a vanished film crew.",
                1999,
                81,
                16,
                "https://example.com/posters/the-blair-witch-project.jpg",
                ["Horror", "Mystery"],
                ["Heather Donahue", "Joshua Leonard"]
            ),
            CreateMovie(
                "Good Luck, Have Fun, Don't Die",
                "A sci-fi adventure where a strange visitor recruits allies for one night.",
                2026,
                115,
                13,
                "https://example.com/posters/good-luck-have-fun-dont-die.jpg",
                ["Sci-Fi", "Adventure"],
                ["Sam Rockwell"]
            ),
            CreateMovie(
                "The Silence of the Lambs",
                "An FBI trainee seeks help from a brilliant imprisoned cannibal.",
                1991,
                118,
                18,
                "https://example.com/posters/the-silence-of-the-lambs.jpg",
                ["Thriller", "Crime"],
                ["Jodie Foster", "Anthony Hopkins"]
            ),
            CreateMovie(
                "Borat",
                "A mockumentary following an outrageous foreign reporter across America.",
                2006,
                84,
                16,
                "https://example.com/posters/borat.jpg",
                ["Comedy"],
                ["Sacha Baron Cohen"]
            ),
            CreateMovie(
                "Memento",
                "A man with short-term memory loss tries to solve his wife's murder.",
                2000,
                113,
                16,
                "https://example.com/posters/memento.jpg",
                ["Thriller", "Mystery"],
                ["Guy Pearce"]
            ),
            CreateMovie(
                "American Psycho",
                "A wealthy New York executive hides a violent double life.",
                2000,
                102,
                18,
                "https://example.com/posters/american-psycho.jpg",
                ["Drama", "Thriller"],
                ["Christian Bale"]
            ),
            CreateMovie(
                "Pulp Fiction",
                "Interwoven crime stories collide in Los Angeles.",
                1994,
                154,
                18,
                "https://example.com/posters/pulp-fiction.jpg",
                ["Crime", "Drama"],
                ["John Travolta", "Uma Thurman", "Samuel L. Jackson"]
            ),
            CreateMovie(
                "Austin Powers: International Man of Mystery",
                "A frozen secret agent awakens to face Dr. Evil.",
                1997,
                89,
                13,
                "https://example.com/posters/austin-powers-international-man-of-mystery.jpg",
                ["Comedy", "Spy"],
                ["Mike Myers"]
            ),
            CreateMovie(
                "Austin Powers: The Spy Who Shagged Me",
                "Austin Powers battles a time-traveling Dr. Evil clone.",
                1999,
                95,
                13,
                "https://example.com/posters/austin-powers-the-spy-who-shagged-me.jpg",
                ["Comedy", "Spy"],
                ["Mike Myers"]
            ),
            CreateMovie(
                "Austin Powers in Goldmember",
                "Austin Powers teams up with his father to stop Goldmember.",
                2002,
                94,
                13,
                "https://example.com/posters/austin-powers-in-goldmember.jpg",
                ["Comedy", "Spy"],
                ["Mike Myers"]
            ),
            CreateMovie(
                "Dune",
                "A noble family becomes entangled in a struggle for the desert planet Arrakis.",
                2021,
                155,
                13,
                "https://example.com/posters/dune-2021.jpg",
                ["Sci-Fi", "Adventure"],
                ["Timothée Chalamet", "Zendaya"]
            ),
            CreateMovie(
                "Dune: Part Two",
                "Paul Atreides unites with the Fremen to avenge his family.",
                2024,
                166,
                13,
                "https://example.com/posters/dune-part-two.jpg",
                ["Sci-Fi", "Adventure"],
                ["Timothée Chalamet", "Zendaya"]
            ),
            CreateMovie(
                "Star Wars: Episode I - The Phantom Menace",
                "Jedi guardians uncover the rise of a new threat.",
                1999,
                136,
                13,
                "https://example.com/posters/star-wars-episode-i.jpg",
                ["Sci-Fi", "Adventure"],
                ["Liam Neeson", "Ewan McGregor"]
            ),
            CreateMovie(
                "Star Wars: Episode II - Attack of the Clones",
                "The galaxy edges closer to war as the clone army emerges.",
                2002,
                142,
                13,
                "https://example.com/posters/star-wars-episode-ii.jpg",
                ["Sci-Fi", "Adventure"],
                ["Ewan McGregor", "Natalie Portman"]
            ),
            CreateMovie(
                "Star Wars: Episode III - Revenge of the Sith",
                "Anakin Skywalker falls toward the dark side.",
                2005,
                140,
                13,
                "https://example.com/posters/star-wars-episode-iii.jpg",
                ["Sci-Fi", "Adventure"],
                ["Ewan McGregor", "Hayden Christensen"]
            ),
            CreateMovie(
                "Star Wars: Episode IV - A New Hope",
                "Rebels race to destroy the Empire's Death Star.",
                1977,
                121,
                10,
                "https://example.com/posters/star-wars-episode-iv.jpg",
                ["Sci-Fi", "Adventure"],
                ["Mark Hamill", "Harrison Ford"]
            ),
            CreateMovie(
                "Star Wars: Episode V - The Empire Strikes Back",
                "The Empire pushes the rebellion to the brink.",
                1980,
                124,
                10,
                "https://example.com/posters/star-wars-episode-v.jpg",
                ["Sci-Fi", "Adventure"],
                ["Mark Hamill", "Harrison Ford"]
            ),
            CreateMovie(
                "Star Wars: Episode VI - Return of the Jedi",
                "The final battle against the Empire begins.",
                1983,
                131,
                10,
                "https://example.com/posters/star-wars-episode-vi.jpg",
                ["Sci-Fi", "Adventure"],
                ["Mark Hamill", "Harrison Ford"]
            ),
            CreateMovie(
                "Star Wars: Episode VII - The Force Awakens",
                "A new generation joins the resistance against the First Order.",
                2015,
                138,
                13,
                "https://example.com/posters/star-wars-episode-vii.jpg",
                ["Sci-Fi", "Adventure"],
                ["Daisy Ridley", "John Boyega"]
            ),
            CreateMovie(
                "Star Wars: Episode VIII - The Last Jedi",
                "Rey seeks guidance while the resistance survives on the run.",
                2017,
                152,
                13,
                "https://example.com/posters/star-wars-episode-viii.jpg",
                ["Sci-Fi", "Adventure"],
                ["Daisy Ridley", "Adam Driver"]
            ),
            CreateMovie(
                "Star Wars: Episode IX - The Rise of Skywalker",
                "The Skywalker saga reaches its final confrontation.",
                2019,
                142,
                13,
                "https://example.com/posters/star-wars-episode-ix.jpg",
                ["Sci-Fi", "Adventure"],
                ["Daisy Ridley", "Adam Driver"]
            ),
            CreateMovie(
                "Fight Club",
                "An insomniac becomes entangled with an underground fight club.",
                1999,
                139,
                18,
                "https://example.com/posters/fight-club.jpg",
                ["Drama", "Thriller"],
                ["Brad Pitt", "Edward Norton"]
            ),
            CreateMovie(
                "Whiplash",
                "A young drummer is pushed to the edge by a ruthless instructor.",
                2014,
                107,
                13,
                "https://example.com/posters/whiplash.jpg",
                ["Drama", "Music"],
                ["Miles Teller", "J. K. Simmons"]
            ),
            CreateMovie(
                "Bugonia",
                "Two conspiracy-obsessed men abduct a powerful executive.",
                2026,
                118,
                16,
                "https://example.com/posters/bugonia.jpg",
                ["Thriller", "Comedy"],
                ["Emma Stone"]
            ),
            CreateMovie(
                "Project Hail Mary",
                "An astronaut wakes up alone to save Earth from extinction.",
                2026,
                130,
                13,
                "https://example.com/posters/project-hail-mary.jpg",
                ["Sci-Fi", "Adventure"],
                ["Ryan Gosling"]
            ),
            CreateMovie(
                "Inception",
                "A thief enters dreams to implant an idea.",
                2010,
                148,
                13,
                "https://example.com/posters/inception.jpg",
                ["Sci-Fi", "Action"],
                ["Leonardo DiCaprio"]
            ),
            CreateMovie(
                "The Matrix",
                "A hacker discovers reality is a simulated prison.",
                1999,
                136,
                16,
                "https://example.com/posters/the-matrix.jpg",
                ["Sci-Fi", "Action"],
                ["Keanu Reeves", "Carrie-Anne Moss"]
            ),
            CreateMovie(
                "Shutter Island",
                "A marshal investigates a disappearance at a remote asylum.",
                2010,
                138,
                16,
                "https://example.com/posters/shutter-island.jpg",
                ["Thriller", "Mystery"],
                ["Leonardo DiCaprio", "Mark Ruffalo"]
            ),
            CreateMovie(
                "VelociPastor",
                "A priest gains the power to become a dinosaur.",
                2018,
                75,
                16,
                "https://example.com/posters/velocipastor.jpg",
                ["Comedy", "Horror"],
                ["Greg Cohan"]
            ),
            CreateMovie(
                "Heat",
                "A detective and a master thief face off in Los Angeles.",
                1995,
                170,
                18,
                "https://example.com/posters/heat.jpg",
                ["Crime", "Drama"],
                ["Al Pacino", "Robert De Niro"]
            )
        };

        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {MovieCount} movies.", movies.Count);
    }

    private static Movie CreateMovie(
        string title,
        string description,
        int releaseYear,
        int duration,
        int ageRating,
        string posterUrl,
        IEnumerable<string> genres,
        IEnumerable<string> actors)
    {
        return Movie.Create(
            title,
            description,
            ReleaseYear.From(releaseYear),
            Duration.From(duration),
            genres.Select(g => new Genre { Value = g }).ToList(),
            actors.Select(a => new Actor { Value = a }).ToList(),
            AgeRating.From(ageRating),
            PosterUrl.From(posterUrl));
    }
}

