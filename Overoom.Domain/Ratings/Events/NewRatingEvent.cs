using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Ratings.Events;

public class NewRatingEvent : IDomainEvent
{
    public NewRatingEvent(Rating rating) => Rating = rating;

    public Rating Rating;
}