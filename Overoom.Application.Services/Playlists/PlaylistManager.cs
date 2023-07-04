using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.Application.Abstractions.Playlists.Exceptions;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Ordering;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications;
using Overoom.Domain.Specifications.Abstractions;
using SortBy = Overoom.Application.Abstractions.Playlists.DTOs.SortBy;

namespace Overoom.Application.Services.Playlists;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlaylistMapper _mapper;

    public PlaylistManager(IUnitOfWork unitOfWork, IPlaylistMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PlaylistShortDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;
        if (!string.IsNullOrEmpty(searchQueryDto.Name))
            specification = new PlaylistByNameSpecification(searchQueryDto.Name);

        IOrderBy<Playlist, IPlaylistSortingVisitor> orderBy =
            searchQueryDto.SortBy == SortBy.Date ? new OrderByUpdateDate() : new OrderByCountFilms();

        if (searchQueryDto.InverseOrder) orderBy = new DescendingOrder<Playlist, IPlaylistSortingVisitor>(orderBy);

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy,
            (searchQueryDto.Page - 1) * 10, 10);
        return playlists.Select(_mapper.MapPlaylistShort).ToList();
    }

    public async Task<List<PlaylistShortDto>> FindByGenresAsync(IReadOnlyCollection<string> genres)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor> specification =
            new PlaylistByGenreSpecification(genres.First());

        specification = genres.Skip(1).Aggregate(specification,
            (current, genre) =>
                new AndSpecification<Playlist, IPlaylistSpecificationVisitor>(current,
                    new PlaylistByGenreSpecification(genre)));

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, new OrderByUpdateDate(),
            0, 10);
        return playlists.Select(_mapper.MapPlaylistShort).ToList();
    }

    public async Task<PlaylistDto> GetAsync(Guid id)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(id);
        if (playlist == null) throw new PlaylistNotFoundException();
        return _mapper.MapPlaylist(playlist);
    }
}