namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;

public sealed record HowestprimeSchedule(
    List<Guid> MovieIds,
    List<MovieEvent> MovieEvents
    );
