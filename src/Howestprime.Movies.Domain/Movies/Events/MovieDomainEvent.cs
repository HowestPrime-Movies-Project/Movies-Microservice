using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.Events;

public class MovieDomainEvent(
    string eventName
    ) : BaseDomainEvent(eventName: eventName,
         aggregateName: nameof(Movie)){}