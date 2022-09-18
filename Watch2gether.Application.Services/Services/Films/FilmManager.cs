using Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;
using Watch2gether.Application.Abstractions.Exceptions.Films;
using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Ordering;
using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Films.Specifications;
using Watch2gether.Domain.Films.Specifications.Visitor;
using Watch2gether.Domain.Ordering;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Specifications;
using Watch2gether.Domain.Specifications.Abstractions;
using SortBy = Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog.SortBy;

namespace Watch2gether.Application.Services.Services.Films;

public class FilmManager : IFilmManager
{
    private readonly IUnitOfWork _unitOfWork;

    public FilmManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FilmLiteDto>> GetFilms(FilmSearchQueryDto searchQueryDto)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Query))
            specification = new FilmFromNameSpecification(searchQueryDto.Query);

        if (!string.IsNullOrEmpty(searchQueryDto.Genre))
        {
            var spec = new FilmFromGenreSpecification(searchQueryDto.Genre);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Person))
        {
            ISpecification<Film, IFilmSpecificationVisitor>
                spec = new FilmFromActorSpecification(searchQueryDto.Person);
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
                new FilmFromDirectorSpecification(searchQueryDto.Person));
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
                new FilmFromScreenWriterSpecification(searchQueryDto.Person));

            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Country))
        {
            var spec = new FilmFromCountrySpecification(searchQueryDto.Country);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (searchQueryDto.MinYear != null || searchQueryDto.MaxYear != null)
        {
            var minYear = searchQueryDto.MinYear ?? 0;
            var maxYear = searchQueryDto.MaxYear ?? int.MaxValue;
            var spec = new FilmFromYearsSpecification(minYear, maxYear);
            specification = specification == null
                ? spec
                : new AndSpecification<Film, IFilmSpecificationVisitor>(specification, spec);
        }

        if (searchQueryDto.Type != null)
        {
            var spec = new FilmFromTypeSpecification(searchQueryDto.Type.Value);
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

    public async Task<FilmDto> GetFilm(Guid id)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
        return MapFilm(film ?? throw new FilmNotFoundException());
    }

    private static FilmDto MapFilm(Film film)
    {
        return new FilmDto(film.Id, film.FilmData.Name, film.FilmData.Year, film.Type, film.PosterFileName,
            film.FilmData.Description ?? "...", film.FilmData.Rating, film.FilmData.Directors,
            film.FilmData.Screenwriters, film.FilmData.Genres, film.FilmData.Countries,
            film.FilmData.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList(), film.FilmData.CountSeasons,
            film.FilmData.CountEpisodes);
    }

    private static FilmLiteDto MapFilms(Film film)
    {
        var description = !string.IsNullOrEmpty(film.FilmData.ShortDescription)
            ? film.FilmData.ShortDescription
            : film.FilmData.Description?[..100] + "...";
        return new FilmLiteDto(film.Id, film.FilmData.Name, film.PosterFileName, film.FilmData.Rating,
            description, film.FilmData.Year, film.Type, film.FilmData.CountSeasons, film.FilmData.Genres);
    }
}