using Overoom.Application.Abstractions.Films.Playlist.DTOs;
using Overoom.Application.Abstractions.Films.Playlist.Exceptions;
using Overoom.Application.Abstractions.Films.Playlist.Interfaces;
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
using SortBy = Overoom.Application.Abstractions.Films.Playlist.DTOs.SortBy;

namespace Overoom.Application.Services.Films.Playlists;

public class PlaylistManager : IPlaylistManager
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        return playlists.Select(MapPlaylistShort).ToList();
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
        return playlists.Select(MapPlaylistShort).ToList();
    }

    public async Task<PlaylistDto> GetAsync(Guid id)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(id);
        if (playlist == null) throw new PlaylistNotFoundException();
        return MapPlaylist(playlist);
    }


    private static PlaylistDto MapPlaylist(Playlist playlist)
    {
        return new PlaylistDto(playlist.Id, playlist.PosterUri, playlist.Updated, playlist.Name,
            playlist.Description, playlist.Genres);
    }

    private static PlaylistShortDto MapPlaylistShort(Playlist playlist)
    {
        return new PlaylistShortDto(playlist.Id, playlist.PosterUri, playlist.Name);
    }
}