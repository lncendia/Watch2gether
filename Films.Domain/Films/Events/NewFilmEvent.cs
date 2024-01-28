using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Films.Events;

public class NewFilmEvent(Film film) : IDomainEvent
{
    public Film Film { get; } = film;
}