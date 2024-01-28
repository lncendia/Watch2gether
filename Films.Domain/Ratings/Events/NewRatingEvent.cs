using Films.Domain.Abstractions;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Ratings.Events;

public class NewRatingEvent : IDomainEvent
{
    public required Guid FilmId { get; init; }
    public required Guid UserId { get; init; }
    public required double Score { get; init; }
}