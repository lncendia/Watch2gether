using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Common.Interfaces;
using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.Specifications;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.PlaylistsManagement;

public class PlaylistManagementService : IPlaylistManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPosterService _playlistPosterService;
    private readonly IPlaylistManagementMapper _mapper;

    public PlaylistManagementService(IUnitOfWork unitOfWork, IPosterService playlistPosterService,
        IPlaylistManagementMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _playlistPosterService = playlistPosterService;
        _mapper = mapper;
    }


    public async Task LoadAsync(LoadDto playlist)
    {
        Uri? poster;
        if (playlist.PosterUri != null) poster = await _playlistPosterService.SaveAsync(playlist.PosterUri);
        else if (playlist.PosterStream != null) poster = await _playlistPosterService.SaveAsync(playlist.PosterStream);
        else throw new PosterMissingException();
        var newPlaylist = new Playlist(poster, playlist.Name, playlist.Description);
        var spec = new FilmByIdsSpecification(playlist.Films.Distinct());
        var count = await _unitOfWork.FilmRepository.Value.CountAsync(spec);
        if (count != playlist.Films.Count) throw new FilmNotFoundException();
        newPlaylist.AddFilms(playlist.Films);
        await _unitOfWork.PlaylistRepository.Value.AddAsync(newPlaylist);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeAsync(ChangeDto change)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(change.Id);
        if (playlist == null) throw new PlaylistNotFoundException();
        if (!string.IsNullOrEmpty(change.Name)) playlist.Name = change.Name;
        if (!string.IsNullOrEmpty(change.Description)) playlist.Description = change.Description;
        Uri? poster = null;
        if (change.PosterUri != null) poster = await _playlistPosterService.SaveAsync(change.PosterUri);
        else if (change.PosterStream != null)
            poster = await _playlistPosterService.SaveAsync(change.PosterStream);

        if (poster != null)
        {
            await _playlistPosterService.DeleteAsync(playlist.PosterUri);
            playlist.PosterUri = poster;
        }

        await _unitOfWork.PlaylistRepository.Value.UpdateAsync(playlist);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid playlistId)
    {
        await _unitOfWork.PlaylistRepository.Value.DeleteAsync(playlistId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PlaylistDto> GetAsync(Guid playlistId)
    {
        var playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(playlistId);
        if (playlist == null) throw new PlaylistNotFoundException();
        var filmSpec = new FilmByIdsSpecification(playlist.Films);
        var films = await _unitOfWork.FilmRepository.Value.FindAsync(filmSpec);
        return _mapper.MapGet(playlist, films);
    }

    public async Task<List<PlaylistShortDto>> FindAsync(int page, string? query = null)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;

        if (!string.IsNullOrEmpty(query)) specification = new PlaylistByNameSpecification(query);

        IOrderBy<Playlist, IPlaylistSortingVisitor> orderBy = new PlaylistOrderByUpdateDate();

        var playlists =
            await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy, (page - 1) * 10, 10);
        return playlists.Select(_mapper.MapShort).ToList();
    }
}