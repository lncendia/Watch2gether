using Films.Domain.Abstractions;

namespace Films.Domain.Films.Events;

/// <summary>
/// Класс, представляющий событие создания нового фильма.
/// </summary>
public class NewFilmDomainEvent(Film film) : IDomainEvent
{
    /// <summary>
    /// Фильм, к которому относится событие.
    /// </summary>
    public Film Film { get; } = film;
}