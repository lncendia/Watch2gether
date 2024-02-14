using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Films.Events;

/// <summary>
/// Класс, представляющий событие создания нового фильма.
/// </summary>
public class NewFilmEvent(Film film) : IDomainEvent
{
    /// <summary>
    /// Фильм, к которому относится событие.
    /// </summary>
    public Film Film { get; } = film;
}