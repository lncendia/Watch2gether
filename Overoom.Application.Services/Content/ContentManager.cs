using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.Application.Abstractions.Content.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.Content;

public class ContentManager : IContentManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContentMapper _mapper;

    public ContentManager(IUnitOfWork unitOfWork, IContentMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<FilmDto>> FindAsync(FilmSearchQuery searchQuery)
    {
        ISpecification<Film, IFilmSpecificationVisitor>? specification = null;

        if (!string.IsNullOrEmpty(searchQuery.Query))
            specification = AddToSpecification(specification, FilmByTitle(searchQuery.Query));

        if (!string.IsNullOrEmpty(searchQuery.Genre))
            specification = AddToSpecification(specification, FilmByGenre(searchQuery.Genre));

        if (!string.IsNullOrEmpty(searchQuery.Person))
            specification = AddToSpecification(specification, FilmByPerson(searchQuery.Person));

        if (!string.IsNullOrEmpty(searchQuery.Country))
            specification = AddToSpecification(specification, FilmByCountry(searchQuery.Country));

        if (searchQuery.MinYear != null || searchQuery.MaxYear != null)
            specification =
                AddToSpecification(specification, FilmByYear(searchQuery.MinYear, searchQuery.MaxYear));

        if (searchQuery.Type != null)
            specification = AddToSpecification(specification, FilmByType(searchQuery.Type.Value));

        if (searchQuery.Playlist != null)
        {
            var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(searchQuery.Playlist.Value);
            if (playlist != null) specification = AddToSpecification(specification, FilmByPlaylist(playlist));
        }

        IOrderBy<Film, IFilmSortingVisitor> orderBy =
            searchQuery.SortBy == FilmSortBy.Rating ? new FilmOrderByRating() : new FilmOrderByDate();
        if (searchQuery.InverseOrder)
            orderBy = new DescendingOrder<Film, IFilmSortingVisitor>(orderBy);

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(specification, orderBy,
            (searchQuery.Page - 1) * 10, 10);
        return films.Select(_mapper.Map).ToList();
    }

    public async Task<List<PlaylistDto>> FindAsync(PlaylistSearchQuery playlistSearchQuery)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(playlistSearchQuery.Name))
            specification =
                AddToSpecification(specification, new PlaylistByNameSpecification(playlistSearchQuery.Name));
        if (!string.IsNullOrEmpty(playlistSearchQuery.Genre))
            specification =
                AddToSpecification(specification, new PlaylistByGenreSpecification(playlistSearchQuery.Genre));


        IOrderBy<Playlist, IPlaylistSortingVisitor> orderBy =
            playlistSearchQuery.SortBy == PlaylistSortBy.Date ? new OrderByUpdateDate() : new OrderByCountFilms();

        if (playlistSearchQuery.InverseOrder) orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(orderBy);

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy,
            (playlistSearchQuery.Page - 1) * 10, 10);
        return playlists.Select(_mapper.Map).ToList();
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

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByGenre(string genre)
    {
        return new FilmByGenreSpecification(genre);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByTitle(string title)
    {
        return new FilmByNameSpecification(title);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByCountry(string country)
    {
        return new FilmByCountrySpecification(country);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByYear(int? minYear,
        int? maxYear)
    {
        return new FilmByYearsSpecification(minYear ?? 0, maxYear ?? int.MaxValue);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByType(FilmType type)
    {
        return new FilmByTypeSpecification(type);
    }

    private static ISpecification<Film, IFilmSpecificationVisitor> FilmByPlaylist(Playlist playlist)
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