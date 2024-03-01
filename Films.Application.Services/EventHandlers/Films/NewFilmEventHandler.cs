using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Posters;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Films.Events;
using Films.Domain.Films.Specifications;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications;
using MediatR;

namespace Films.Application.Services.EventHandlers.Films;

public class NewFilmEventHandler(IUnitOfWork unitOfWork, IPosterStore posterStore)
    : INotificationHandler<NewFilmDomainEvent>
{
    public async Task Handle(NewFilmDomainEvent notification, CancellationToken cancellationToken)
    {
        var filmSpec = new AndSpecification<Film, IFilmSpecificationVisitor>(
            new FilmsByTitleSpecification(notification.Film.Title),
            new FilmsByYearsSpecification(notification.Film.Year, notification.Film.Year));

        var count = await unitOfWork.FilmRepository.Value.FindAsync(filmSpec);
        if (count.Count > 0)
        {
            await posterStore.DeleteAsync(notification.Film.PosterUrl);
            throw new FilmAlreadyExistsException();
        }
    }
}