using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Common.Interfaces;
using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Application.Services.PlaylistsManagement;

public class PlaylistManagementService : IPlaylistManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPosterService _playlistPosterService;
    private readonly IMemoryCache _memoryCache;
    private readonly IPlaylistManagementMapper _mapper;

    public PlaylistManagementService(IUnitOfWork unitOfWork, IPosterService playlistPosterService, IMemoryCache memoryCache,
        IPlaylistManagementMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _playlistPosterService = playlistPosterService;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task LoadAsync(LoadDto playlist)
    {
        Uri? poster;
        if (playlist.PosterUri != null) poster = await _playlistPosterService.SaveAsync(playlist.PosterUri);
        else if (playlist.PosterStream != null) poster = await _playlistPosterService.SaveAsync(playlist.PosterStream);
        else throw new PosterMissingException();
        var builder = PlaylistBuilder.Create()
            .WithName(playlist.Name)
            .WithDescription(playlist.Description)
            .WithYear(playlist.Year)
            .WithRating(playlist.Rating)
            .WithType(playlist.Type)
            .WithCdn(playlist.CdnList.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)))
            .WithGenres(playlist.Genres)
            .WithActors(playlist.Actors)
            .WithDirectors(playlist.Directors)
            .WithScreenwriters(playlist.Screenwriters)
            .WithCountries(playlist.Countries)
            .WithPoster(poster);
        if (!string.IsNullOrEmpty(playlist.ShortDescription)) builder = builder.WithShortDescription(playlist.ShortDescription);
        if (playlist is { CountSeasons: not null, CountEpisodes: not null })
            builder = builder.WithEpisodes(playlist.CountSeasons.Value, playlist.CountEpisodes.Value);
        await _unitOfWork.PlaylistRepository.Value.AddAsync(builder.Build());
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeAsync(ChangeDto toChange)
    {
        var playlist = await GetPlaylistAsync(toChange.PlaylistId);
        if (!string.IsNullOrEmpty(toChange.Description)) playlist.Description = toChange.Description;
        if (!string.IsNullOrEmpty(toChange.ShortDescription)) playlist.ShortDescription = toChange.ShortDescription;
        if (toChange.Rating.HasValue) playlist.Rating = toChange.Rating.Value;
        if (toChange is { CountSeasons: not null, CountEpisodes: not null })
            playlist.UpdateSeriesInfo(toChange.CountSeasons.Value, toChange.CountEpisodes.Value);

        Uri? poster = null;
        if (toChange.PosterUri != null) poster = await _playlistPosterService.SaveAsync(toChange.PosterUri);
        else if (toChange.PosterStream != null)
            poster = await _playlistPosterService.SaveAsync(toChange.PosterStream);

        if (poster != null)
        {
            await _playlistPosterService.DeleteAsync(playlist.PosterUri);
            playlist.PosterUri = poster;
        }

        if (toChange.CdnList != null)
        {
            foreach (var cdnDto in toChange.CdnList)
            {
                playlist.AddOrChangeCdn(new CdnDto(cdnDto.Type, cdnDto.Uri, cdnDto.Quality, cdnDto.Voices));
            }
        }

        await _unitOfWork.PlaylistRepository.Value.UpdateAsync(playlist);
        await _unitOfWork.SaveChangesAsync();
        _memoryCache.Remove(playlist.Id);
    }

    public async Task DeleteAsync(Guid playlistId)
    {
        var playlist = await GetPlaylistAsync(playlistId);
        await _playlistPosterService.DeleteAsync(playlist.PosterUri);
        await _unitOfWork.PlaylistRepository.Value.DeleteAsync(playlistId);
        await _unitOfWork.SaveChangesAsync();
        _memoryCache.Remove(playlistId);
    }

    public async Task<PlaylistDto> GetAsync(Guid playlistId)
    {
        var playlist = await GetPlaylistAsync(playlistId);
        return _mapper.MapGet(playlist);
    }

    public async Task<List<PlaylistShortDto>> FindAsync(int page, string? query = null)
    {
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification = null;

        if (!string.IsNullOrEmpty(query)) specification = new PlaylistByNameSpecification(query);

        IOrderBy<Playlist, IPlaylistSortingVisitor> orderBy = new PlaylistOrderByDate();

        var playlists = await _unitOfWork.PlaylistRepository.Value.FindAsync(specification, orderBy, (page - 1) * 10, 10);
        return playlists.Select(_mapper.MapShort).ToList();
    }


    private async Task<Playlist> GetPlaylistAsync(Guid id)
    {
        if (!_memoryCache.TryGetValue(id, out Playlist? playlist))
        {
            playlist = await _unitOfWork.PlaylistRepository.Value.GetAsync(id);
            if (playlist == null) throw new PlaylistNotFoundException();
            _memoryCache.Set(id, playlist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        else
        {
            if (playlist == null) throw new PlaylistNotFoundException();
        }

        return playlist;
    }
}