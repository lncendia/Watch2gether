using Overoom.Domain.Abstractions;
using Overoom.Domain.Ratings.Events;
using Overoom.Domain.Ratings.Exceptions;

namespace Overoom.Domain.Ratings.Entities;

public class Rating : AggregateRoot
{
    public Rating(Guid filmId, Guid userId, double score)
    {
        FilmId = filmId;
        UserId = userId;
        if (score is < 0 or > 10) throw new ScoreException();
        Score = score;
        AddDomainEvent(new NewRatingEvent(this));
    }

    public Guid FilmId { get; }
    public Guid? UserId { get; }
    public double Score { get; }
    public DateTime Date { get; } = DateTime.UtcNow;
}