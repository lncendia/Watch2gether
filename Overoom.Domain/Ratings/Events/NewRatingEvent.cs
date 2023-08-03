using Overoom.Domain.Abstractions;
using Overoom.Domain.Ratings.Entities;

namespace Overoom.Domain.Ratings.Events;

public class NewRatingEvent : IDomainEvent
{
    public NewRatingEvent(Rating rating) => Rating = rating;

    public Rating Rating { get; }
}