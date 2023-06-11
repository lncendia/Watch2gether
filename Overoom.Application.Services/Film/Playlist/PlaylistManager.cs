using Overoom.Application.Abstractions.Film.Playlist.DTOs;
using Overoom.Application.Abstractions.Film.Playlist.Exceptions;
using Overoom.Application.Abstractions.Film.Playlist.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlist.Ordering;
using Overoom.Domain.Playlist.Ordering.Visitor;
using Overoom.Domain.Playlist.Specifications;
using Overoom.Domain.Playlist.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using SortBy = Overoom.Application.Abstractions.Film.Playlist.DTOs.SortBy;

namespace Overoom.Application.Services.Film.Playlist;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PlaylistDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto)
    {
        ISpecification<Domain.Playlist.Entities.Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Name))
            specification = new PlaylistByNameSpecification(searchQueryDto.Name);

        IOrderBy<Domain.Playlist.Entities.Playlist, IPlaylistSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Date ? new OrderByUpdateDate() : new OrderByCountFilms();
        if (searchQueryDto.InverseOrder)
            orderBy = new DescendingOrder<Domain.Playlist.Entities.Playlist, IPlaylistSortingVisitor>(orderBy);

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
    

    private static PlaylistDto MapPlaylist(Domain.Playlist.Entities.Playlist playlist)
    {
        return new PlaylistDto(playlist.Id, playlist.PosterUri, playlist.Updated, playlist.Name,
            playlist.Description);
    }
}