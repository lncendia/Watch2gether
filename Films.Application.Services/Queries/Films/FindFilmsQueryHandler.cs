using MediatR;
using Films.Application.Abstractions.Queries.Films;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Domain.Abstractions.Repositories.UnitOfWorks;
using Films.Domain.Films.Entities;
using Films.Domain.Films.Enums;
using Films.Domain.Films.Ordering;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Films.Specifications;
using Films.Domain.Films.Specifications.Visitor;
using Films.Domain.Ordering;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists.Entities;
using Films.Domain.Specifications;
using Films.Domain.Specifications.Abstractions;

namespace Films.Application.Services.Queries.Films;

public class FindFilmsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindFilmsQuery, IReadOnlyCollection<FilmShortDto>>
{
    public async Task<IReadOnlyCollection<FilmShortDto>> Handle(FindFilmsQuery request, CancellationToken cancellationToken)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;

        // Добавляем спецификации в соответствии с заданными параметрами поиска 
        if (!string.IsNullOrEmpty(request.Query))
            specification = AddToSpecification(specification, FilmByTitle(request.Query));

        if (!string.IsNullOrEmpty(request.Genre))
            specification = AddToSpecification(specification, FilmByGenre(request.Genre));

        if (!string.IsNullOrEmpty(request.Person))
            specification = AddToSpecification(specification, FilmByPerson(request.Person));

        if (!string.IsNullOrEmpty(request.Country))
            specification = AddToSpecification(specification, FilmByCountry(request.Country));

        if (request.Type != null)
            specification = AddToSpecification(specification, FilmByType(request.Type.Value));

        if (request.Playlist != null)
        {
            var playlist = await unitOfWork.PlaylistRepository.Value.GetAsync(request.Playlist.Value);
            if (playlist != null) specification = AddToSpecification(specification, FilmByPlaylist(playlist));
        }

        // Определяем порядок сортировки по дате 
        IOrderBy<Film, IFilmSortingVisitor> orderBy =
            new DescendingOrder<Film, IFilmSortingVisitor>(new FilmOrderByDate());

        // Получаем список фильмов из репозитория с применением спецификаций, сортировки и ограничением на количество 
        var films = await unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy, request.Skip, request.Take);

        // Преобразуем фильмы в список DTO фильмов 
        return films.Select(Map).ToArray();
    }

    private static FilmShortDto Map(Film film)
    {
        return new FilmShortDto
        {
            Id = film.Id,
            Name = film.Title,
            PosterUrl = film.PosterUrl,
            Year = film.Year,
            Rating = film.UserRating,
            ShortDescription = film.ShortDescription!,
            Type = film.Type,
            Genres = film.Genres,
            CountSeasons = film.CountSeasons
        };
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByPerson(string person)
    {
        ISpecification<Film, IFilmSpecificationVisitor>
            spec = new FilmByActorSpecification(person);

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
            new FilmByDirectorSpecification(person));

        spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
            new FilmByScreenWriterSpecification(person));
        return spec;
    }

    private static FilmByGenreSpecification FilmByGenre(string genre)
    {
        return new FilmByGenreSpecification(genre);
    }

    private static FilmByNameSpecification FilmByTitle(string title)
    {
        return new FilmByNameSpecification(title);
    }

    private static FilmByCountrySpecification FilmByCountry(string country)
    {
        return new FilmByCountrySpecification(country);
    }

    private static FilmByTypeSpecification FilmByType(FilmType type)
    {
        return new FilmByTypeSpecification(type);
    }

    private static FilmByIdsSpecification FilmByPlaylist(Playlist playlist)
    {
        return new FilmByIdsSpecification(playlist.Films);
    }

    private static ISpecification<T, TV> AddToSpecification<T, TV>(
        ISpecification<T, TV>? baseSpec, ISpecification<T, TV> newSpec) where TV : ISpecificationVisitor<TV, T>
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<T, TV>(baseSpec, newSpec);
    }
}