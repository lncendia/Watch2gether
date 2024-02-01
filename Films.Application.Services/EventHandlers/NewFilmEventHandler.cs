using Films.Application.Abstractions.Common.Exceptions;
using Films.Application.Abstractions.Common.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Events;
using Films.Domain.Films.Specifications;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Specifications;

namespace Films.Application.Services.EventHandlers;

public class NewFilmEventHandler(IUnitOfWork unitOfWork, IPosterService posterService)
    : INotificationHandler<NewFilmEvent>
{
    public async Task Handle(NewFilmEvent notification, CancellationToken cancellationToken)
    {
        var filmSpec = new AndSpecification<Film, IFilmSpecificationVisitor>(
            new FilmByNameSpecification(notification.Film.Title),
            new FilmByYearsSpecification(notification.Film.Year, notification.Film.Year));

        var count = await unitOfWork.FilmRepository.Value.FindAsync(filmSpec);
        if (count.Count > 0)
        {
            await posterService.DeleteAsync(notification.Film.PosterUrl);
            throw new FilmAlreadyExistsException();
        }
    }
}