using Films.Domain.Abstractions;
using Films.Domain.Films;
using Films.Domain.Users;

namespace Films.Domain.Ratings.Events;

public class NewRatingDomainEvent : IDomainEvent
{
    public required Rating Rating { get; init; }
    public required Film Film { get; init; }
    public required User User { get; init; }
}