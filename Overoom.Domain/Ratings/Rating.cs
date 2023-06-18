using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Ratings;

public class Rating : AggregateRoot
{
    public Rating(Guid filmId, Guid userId, double score)
    {
        FilmId = filmId;
        UserId = userId;
        Score = score;
    }

    public Guid FilmId { get; }
    public Guid? UserId { get; }
    public double Score { get; }
}