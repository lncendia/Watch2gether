using MediatR;
using Overoom.Application.Abstractions.Movie.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Events;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Specifications;

namespace Overoom.Application.Services.Movie.EventHandlers;

public class NewFilmEventHandler : INotificationHandler<NewFilmEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public NewFilmEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(NewFilmEvent notification, CancellationToken cancellationToken)
    {
        var filmSpec = new AndSpecification<Film, IFilmSpecificationVisitor>(
            new FilmByNameSpecification(notification.Film.Name),
            new FilmByYearsSpecification(notification.Film.Year, notification.Film.Year));

        var count = await _unitOfWork.FilmRepository.Value.FindAsync(filmSpec);
        if (count.Count > 0) throw new FilmAlreadyExistsException();
    }
}