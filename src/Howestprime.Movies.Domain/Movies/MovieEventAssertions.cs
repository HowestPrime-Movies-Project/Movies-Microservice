namespace Howestprime.Movies.Domain.Movies;

public class MovieEventAssertions
{
    public static void EnsureShowtimeIsAt15hOr19h(DateTime time)
    {
        if (time.Hour != 15 && time.Hour != 19)
            throw new ArgumentException("Time must be at 15h or 19h.", nameof(time));
    }

    public static void EnsureShowtimeIsInTheFuture(DateTime time)
    {
        DateTime reference = time.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
        if (time <= reference)
            throw new ArgumentException($"Movie event {time} must be in the future.");
    }

}