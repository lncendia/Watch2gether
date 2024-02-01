using Films.Domain.Abstractions;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Ratings.Events;

public class NewRatingEvent(Rating rating) : IDomainEvent
{
    public Rating Rating { get;  } = rating;
}