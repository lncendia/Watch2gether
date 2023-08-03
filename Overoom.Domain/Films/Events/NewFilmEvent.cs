using Overoom.Domain.Abstractions;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings.Entities;

namespace Overoom.Domain.Films.Events;

public class NewFilmEvent : IDomainEvent
{
    public NewFilmEvent(Film film) => Film = film;
    public Film Film { get; }
}