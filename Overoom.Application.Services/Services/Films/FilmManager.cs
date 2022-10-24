using Overoom.Application.Abstractions.DTO.Films.FilmCatalog;
using Overoom.Application.Abstractions.Exceptions.Films;
using Overoom.Application.Abstractions.Interfaces.Films;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using SortBy = Overoom.Application.Abstractions.DTO.Films.FilmCatalog.SortBy;

namespace Overoom.Application.Services.Services.Films;

public class FilmManager : IFilmManager
{
    private readonly IUnitOfWork _unitOfWork;

    public FilmManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FilmLiteDto>> GetFilmsAsync(FilmSearchQueryDto searchQueryDto)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Query))
            specification = new FilmByNameSpecification(searchQueryDto.Query);

        if (!string.IsNullOrEmpty(searchQueryDto.Genre))
        {
            var spec = new FilmByGenreSpecification(searchQueryDto.Genre);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Person))
        {
            ISpecification<Film, IFilmSpecificationVisitor>
                spec = new FilmByActorSpecification(searchQueryDto.Person);
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
                new FilmByDirectorSpecification(searchQueryDto.Person));
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
                new FilmByScreenWriterSpecification(searchQueryDto.Person));

            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Country))
        {
            var spec = new FilmByCountrySpecification(searchQueryDto.Country);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (searchQueryDto.MinYear != null || searchQueryDto.MaxYear != null)
        {
            var minYear = searchQueryDto.MinYear ?? 0;
            var maxYear = searchQueryDto.MaxYear ?? int.MaxValue;
            var spec = new FilmByYearsSpecification(minYear, maxYear);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (searchQueryDto.Type != null)
        {
            var spec = new FilmByTypeSpecification(searchQueryDto.Type.Value);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        IOrderBy<Film, IFilmSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Rating ? new OrderByRating() : new OrderByDate();
        if (searchQueryDto.InverseOrder) orderBy = new DescendingOrder<Film, IFilmSortingVisitor>(orderBy);

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy,
            (searchQueryDto.Page - 1) * 10,
            10);
        return films.Select(MapFilms).ToList();
    }

    public async Task<FilmDto> GetFilmAsync(Guid id)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
        return MapFilm(film ?? throw new FilmNotFoundException());
    }

    public async Task DeleteFilmAsync(Guid id)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
        if (film == null) throw new FilmNotFoundException();
        await _unitOfWork.FilmRepository.Value.DeleteAsync(film);
        await _unitOfWork.SaveAsync();
    }

    private static FilmDto MapFilm(Film film)
    {
        return new FilmDto(film.Id, film.Name, film.Date.Year, film.Type, film.PosterFileName,
            film.FilmInfo.Description ?? "...", film.FilmInfo.Rating, film.FilmCollections.Directors,
            film.FilmCollections.Screenwriters, film.FilmCollections.Genres, film.FilmCollections.Countries,
            film.FilmCollections.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList(),
            film.FilmInfo.CountSeasons,
            film.FilmInfo.CountEpisodes, film.Url.ToString());
    }

    private static FilmLiteDto MapFilms(Film film)
    {
        var description = !string.IsNullOrEmpty(film.FilmInfo.ShortDescription)
            ? film.FilmInfo.ShortDescription
            : film.FilmInfo.Description?[..100] + "...";
        return new FilmLiteDto(film.Id, film.Name, film.PosterFileName, film.FilmInfo.Rating,
            description, film.Date.Year, film.Type, film.FilmInfo.CountSeasons, film.FilmCollections.Genres);
    }
}