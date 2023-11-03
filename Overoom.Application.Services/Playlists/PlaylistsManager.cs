using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Ordering;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;

namespace Overoom.Application.Services.Playlists;

public class PlaylistsManager : IPlaylistsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlaylistsMapper _mapper;

    public PlaylistsManager(IUnitOfWork unitOfWork, IPlaylistsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PlaylistDto>> FindAsync(PlaylistSearchQuery query)
    {
        var orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(new PlaylistOrderByUpdateDate());

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(null, orderBy, (query.Page - 1) * 10, 10);
        return playlists.Select(_mapper.Map).ToList();
    }
}