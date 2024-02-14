using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Events;
using Films.Domain.Ratings.Exceptions;
using Films.Domain.Users.Entities;

namespace Films.Domain.Ratings.Entities;

/// <summary>
/// Класс, представляющий оценку фильма от пользователя.
/// </summary>
public class Rating : AggregateRoot
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Rating"/>.
    /// </summary>
    /// <param name="film">Фильм, который пользователь оценивает.</param>
    /// <param name="user">Пользователь, который оставляет оценку.</param>
    /// <param name="score">Оценка фильма.</param>
    /// <exception cref="ScoreException">Выбрасывается, если оценка находится вне диапазона от 0 до 10.</exception>
    public Rating(Film film, User user, double score)
    {
        FilmId = film.Id;
        UserId = user.Id;
        if (score is < 0 or > 10) throw new ScoreException();
        Score = score;
        AddDomainEvent(new NewRatingEvent
        {
            Rating = this,
            Film = film
        });
    }

    /// <summary>
    /// Идентификатор фильма, к которому относится оценка.
    /// </summary>
    public Guid FilmId { get; }

    /// <summary>
    /// Идентификатор пользователя, оставившего оценку (может быть null, если пользователь неавторизован).
    /// </summary>
    public Guid? UserId { get; }

    /// <summary>
    /// Оценка фильма.
    /// </summary>
    public double Score { get; }

    /// <summary>
    /// Дата оценки.
    /// </summary>
    public DateTime Date { get; } = DateTime.UtcNow;
}