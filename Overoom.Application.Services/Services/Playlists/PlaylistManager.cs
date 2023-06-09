using Overoom.Application.Abstractions.DTO.Playlists;
using Overoom.Application.Abstractions.Exceptions.Playlists;
using Overoom.Application.Abstractions.Interfaces.Playlists;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Films.Specifications.Visitor;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.Services.Playlists;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistManager(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<PlaylistLiteDto>> GetPlaylists(PlaylistSearchQueryDto searchQueryDto)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Name))
            specification = new PlaylistByNameSpecification(searchQueryDto.Name);

        IOrderBy<Playlist, IPlaylistSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Date ? new OrderByUpdateDate() : new OrderByCountFilms();
        if (searchQueryDto.InverseOrder) orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(orderBy);

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy,
            (searchQueryDto.Page - 1) * 10,
            10);
        return playlists.Select(MapPlaylists).ToList();
    }

    public async Task<PlaylistDto> GetPlaylist(Guid id)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(id);
        if (playlist == null) throw new PlaylistNotFoundException();

        ISpecification<Film, IFilmSpecificationVisitor> spec = new FilmByIdSpecification(playlist.Films[0]);
        for (var i = 1; i < playlist.Films.Count; i++)
        {
            spec = new OrSpecification<Film, IFilmSpecificationVisitor>(spec,
                new FilmByIdSpecification(playlist.Films[i]));
        }

        var films = await _unitOfWork.FilmRepository.Value.FindAsync(spec);
        return MapPlaylist(playlist, films);
    }

    private static PlaylistDto MapPlaylist(Playlist playlist, IEnumerable<Film> films)
    {
        var filmsDto = films.Select(f => new PlaylistFilmLiteDto(f.Id, f.Name, f.PosterUri,
                f.FilmInfo.Rating,
                !string.IsNullOrEmpty(f.FilmInfo.ShortDescription)
                    ? f.FilmInfo.ShortDescription
                    : f.FilmInfo.Description?[..100] + "...", f.Date.Year, f.Type, f.FilmInfo.CountSeasons,
                f.FilmCollections.Genres))
            .ToList();
        return new PlaylistDto(playlist.Id, playlist.PosterUri, playlist.Updated, filmsDto, playlist.Name,
            playlist.Description);
    }

    private static PlaylistLiteDto MapPlaylists(Playlist playlist) => new(playlist.Id, playlist.Name,
        playlist.PosterUri, playlist.Films.Count, playlist.Updated);
}