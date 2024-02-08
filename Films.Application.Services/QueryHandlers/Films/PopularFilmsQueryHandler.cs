using Films.Application.Abstractions.Queries.Films;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Ordering;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Ordering;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Films;

public class PopularFilmsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<PopularFilmsQuery, IReadOnlyCollection<FilmShortDto>>
{
    public async Task<IReadOnlyCollection<FilmShortDto>> Handle(PopularFilmsQuery request,
        CancellationToken cancellationToken)
    {
        var date = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate());
        var rating = new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByUserRatingCount());
        var order = new ThenByOrder<Film, IFilmSortingVisitor>(date, rating);
        var films = await unitOfWork.FilmRepository.Value.FindAsync(null, order, 0, 15);

        // Преобразуем фильмы в список DTO фильмов 
        return films.Select(Mapper.Map).ToArray();
    }
}