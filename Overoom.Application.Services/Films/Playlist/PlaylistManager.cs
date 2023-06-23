using Overoom.Application.Abstractions.Films.Playlist.DTOs;
using Overoom.Application.Abstractions.Films.Playlist.Exceptions;
using Overoom.Application.Abstractions.Films.Playlist.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using SortBy = Overoom.Application.Abstractions.Films.Playlist.DTOs.SortBy;

namespace Overoom.Application.Services.Films.Playlist;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PlaylistDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto)
    {
        ISpecification<Domain.Playlists.Entities.Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Name))
            specification = new PlaylistByNameSpecification(searchQueryDto.Name);

        IOrderBy<Domain.Playlists.Entities.Playlist, IPlaylistSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Date ? new OrderByUpdateDate() : new OrderByCountFilms();
        if (searchQueryDto.InverseOrder)
            orderBy = new DescendingOrder<Domain.Playlists.Entities.Playlist, IPlaylistSortingVisitor>(orderBy);

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy,
            (searchQueryDto.Page - 1) * 10,
            10);
        return playlists.Select(MapPlaylist).ToList();
    }

    public async Task<PlaylistDto> GetAsync(Guid id)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(id);
        if (playlist == null) throw new PlaylistNotFoundException();
        return MapPlaylist(playlist);
    }
    

    private static PlaylistDto MapPlaylist(Domain.Playlists.Entities.Playlist playlist)
    {
        return new PlaylistDto(playlist.Id, playlist.PosterUri, playlist.Updated, playlist.Name,
            playlist.Description);
    }
}