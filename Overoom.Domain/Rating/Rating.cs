namespace Overoom.Domain.Rating;

public class Rating
{
    public Rating(Guid filmId, Guid userId, double score)
    {
        Id = Guid.NewGuid();
        FilmId = filmId;
        UserId = userId;
        Score = score;
    }

    public Guid Id { get; }
    public Guid FilmId { get; }
    public Guid UserId { get; }
    public double Score { get; }
}