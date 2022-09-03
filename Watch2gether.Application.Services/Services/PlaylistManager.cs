using Watch2gether.Application.Abstractions.DTO.Playlists;
using Watch2gether.Application.Abstractions.Exceptions.Playlists;
using Watch2gether.Application.Abstractions.Interfaces.Playlists;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Specifications;
using Watch2gether.Domain.Playlists;
using Watch2gether.Domain.Playlists.Ordering;
using Watch2gether.Domain.Playlists.Ordering.Visitor;
using Watch2gether.Domain.Playlists.Specifications;
using Watch2gether.Domain.Playlists.Specifications.Visitor;
using Watch2gether.Domain.Ordering;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Application.Services.Services;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PlaylistLiteDto>> GetPlaylists(PlaylistSearchQueryDto searchQueryDto)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Name))
            specification = new PlaylistFromNameSpecification(searchQueryDto.Name);

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
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(new FilmFromIdsSpecification(playlist.Films));
        return MapPlaylist(playlist, films);
    }

    private static PlaylistDto MapPlaylist(Playlist playlist, IEnumerable<Film> films)
    {
        var filmsDto = films.Select(f => new PlaylistFilmLiteDto(f.Id, f.FilmData.Name, f.PosterFileName,
                f.FilmData.Rating,
                !string.IsNullOrEmpty(f.FilmData.ShortDescription)
                    ? f.FilmData.ShortDescription
                    : f.FilmData.Description?[..100] + "...", f.FilmData.Year, f.Type, f.FilmData.CountSeasons,
                f.FilmData.Genres))
            .ToList();
        return new PlaylistDto(playlist.Id, playlist.PosterFileName, playlist.Updated, filmsDto, playlist.Name,
            playlist.Description);
    }

    private static PlaylistLiteDto MapPlaylists(Playlist playlist) => new PlaylistLiteDto(playlist.Id, playlist.Name,
        playlist.PosterFileName, playlist.Films.Count, playlist.Updated);
}