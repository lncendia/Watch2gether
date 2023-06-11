using Overoom.Application.Abstractions.Film.Catalog.DTOs;
using Overoom.Application.Abstractions.Film.Catalog.Exceptions;
using Overoom.Application.Abstractions.Film.Catalog.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Film.Enums;
using Overoom.Domain.Film.Ordering;
using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Film.Specifications;
using Overoom.Domain.Film.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using SortBy = Overoom.Application.Abstractions.Film.Catalog.DTOs.SortBy;

namespace Overoom.Application.Services.Film.Catalog;

public class FilmManager : IFilmManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmMapper _mapper;

    public FilmManager(IUnitOfWork unitOfWork, IFilmMapper filmMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = filmMapper;
    }

    public async Task<List<FilmShortDto>> FindAsync(FilmSearchQueryDto searchQueryDto)
    {
        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>? specification = null;

        if (!string.IsNullOrEmpty(searchQueryDto.Query))
        {
            specification = AddToSpecification(specification, FilmByTitle(searchQueryDto.Query));
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Genre))
        {
            specification = AddToSpecification(specification, FilmByGenre(searchQueryDto.Genre));
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Person))
        {
            specification = AddToSpecification(specification, FilmByPerson(searchQueryDto.Person));
        }

        if (!string.IsNullOrEmpty(searchQueryDto.Country))
        {
            specification = AddToSpecification(specification, FilmByCountry(searchQueryDto.Country));
        }

        if (searchQueryDto.MinYear != null || searchQueryDto.MaxYear != null)
        {
            specification =
                AddToSpecification(specification, FilmByYear(searchQueryDto.MinYear, searchQueryDto.MaxYear));
        }

        if (searchQueryDto.Type != null)
        {
            specification = AddToSpecification(specification, FilmByType(searchQueryDto.Type.Value));
        }

        if (searchQueryDto.Playlist != null)
        {
            var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(searchQueryDto.Playlist.Value);
            if (playlist != null) specification = AddToSpecification(specification, FilmByPlaylist(playlist));
        }

        IOrderBy<Domain.Film.Entities.Film, IFilmSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Rating ? new OrderByRating() : new OrderByDate();
        if (searchQueryDto.InverseOrder)
            orderBy = new DescendingOrder<Domain.Film.Entities.Film, IFilmSortingVisitor>(orderBy);

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy,
            (searchQueryDto.Page - 1) * 10, 10);
        return films.Select(_mapper.MapFilmShort).ToList();
    }

    public async Task<FilmDto> GetAsync(Guid id)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(id);
        return _mapper.MapFilm(film ?? throw new FilmNotFoundException());
    }


    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByPerson(string person)
    {
        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>
            spec = new FilmByActorSpecification(person);

        spec = new OrSpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>(spec,
            new FilmByDirectorSpecification(person));

        spec = new OrSpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>(spec,
            new FilmByScreenWriterSpecification(person));
        return spec;
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByGenre(string genre)
    {
        return new FilmByGenreSpecification(genre);
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByTitle(string title)
    {
        return new FilmByNameSpecification(title);
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByCountry(string country)
    {
        return new FilmByCountrySpecification(country);
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByYear(int? minYear,
        int? maxYear)
    {
        return new FilmByYearsSpecification(minYear ?? 0, maxYear ?? int.MaxValue);
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByType(FilmType type)
    {
        return new FilmByTypeSpecification(type);
    }

    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> FilmByPlaylist(
        Domain.Playlist.Entities.Playlist playlist)
    {
        return new FilmByIdsSpecification(playlist.Films);
    }
    
    private static ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> AddToSpecification(
        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>? baseSpec,
        ISpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor> newSpec)
    {
        return baseSpec == null
            ? newSpec
            : new AndSpecification<Domain.Film.Entities.Film, IFilmSpecificationVisitor>(baseSpec, newSpec);
    }
}