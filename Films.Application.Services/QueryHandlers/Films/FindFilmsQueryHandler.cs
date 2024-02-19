using Films.Application.Abstractions.Queries.Films;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Application.Services.Common;
using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Films.Ordering;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Films.Specifications;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Ordering;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Specifications;
using Films.Domain.Specifications.Abstractions;
using MediatR;

namespace Films.Application.Services.QueryHandlers.Films;

public class FindFilmsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FindFilmsQuery, (IReadOnlyCollection<FilmShortDto> films, int count)>
{
    public async Task<(IReadOnlyCollection<FilmShortDto> films, int count)> Handle(FindFilmsQuery request,
        CancellationToken cancellationToken)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(request.Query))
            specification = specification.AddToSpecification(new FilmsByTitleSpecification(request.Query));

        if (!string.IsNullOrEmpty(request.Genre)) 
            specification = specification.AddToSpecification(new FilmsByGenreSpecification(request.Genre));

        if (!string.IsNullOrEmpty(request.Person))
            specification = specification.AddToSpecification(FilmByPerson(request.Person));

        if (!string.IsNullOrEmpty(request.Country))
            specification = specification.AddToSpecification(new FilmsByCountrySpecification(request.Country));

        if (request.Serial != null) 
            specification = specification.AddToSpecification(new FilmsByTypeSpecification(request.Serial.Value));

        if (request.PlaylistId != null)
        {
            var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.PlaylistId.Value);
            if (playlist != null) specification = specification.AddToSpecification(new FilmsByIdsSpecification(playlist.Films));
        }

        // Определяем порядок сортировки по дате 
        IOrderBy<Film, IFilmSortingVisitor> orderBy =
            new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate());

        // Получаем список фильмов из репозитория с применением спецификаций, сортировки и ограничением на количество 
        var films = await unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy, request.Skip, request.Take);

        if (films.Count == 0) return ([], 0);

        var count = await unitOfWork.FilmRepository.Value.CountAsync(specification);

        // Преобразуем фильмы в список DTO фильмов 
        return (films.Select(Mapper.Map).ToArray(), count);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByPerson(string person)
    {
        ISpecification<Film, IFilmSpecificationVisitor> spec = new FilmsByActorSpecification(person);

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec, new FilmsByDirectorSpecification(person));

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec, new FilmsByScreenWriterSpecification(person));

        return spec;
    }
}