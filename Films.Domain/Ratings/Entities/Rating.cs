using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Exceptions;
using Films.Domain.Users.Entities;

namespace Films.Domain.Ratings.Entities;

public class Rating : AggregateRoot
{
    public Rating(Film film, User user, double score)
    {
        FilmId = film.Id;
        UserId = user.Id;
        if (score is < 0 or > 10) throw new ScoreException();
        Score = score;
        AddDomainEvent(new NewRatingEvent(this));
    }

    public Guid FilmId { get; }
    public Guid? UserId { get; }
    public double Score { get; }
    public DateTime Date { get; } = DateTime.UtcNow;
}