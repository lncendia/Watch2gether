using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Ratings.Events;

public class NewRatingEvent : IDomainEvent
{
    public required Rating Rating { get; init; }
    public required Film Film { get; init; }
}