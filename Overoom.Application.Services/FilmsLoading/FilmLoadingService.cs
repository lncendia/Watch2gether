using Overoom.Application.Abstractions.Common.Interfaces;
using Overoom.Application.Abstractions.FilmsLoading;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Application.Abstractions.MovieApi.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Films.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Services.FilmsLoading;

public class FilmAutoLoader : IFilmAutoLoader
{
    private readonly IKpApiService _kpApi;
    private readonly IVideoCdnApiService _videoCdnApi;
    private readonly IBazonApiService _bazonApiService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPosterService _posterService;

    public FilmAutoLoader(IKpApiService kpApi, IVideoCdnApiService videoCdnApi, IBazonApiService bazonApiService,
        IUnitOfWork unitOfWork, IPosterService posterService)
    {
        _kpApi = kpApi;
        _videoCdnApi = videoCdnApi;
        _bazonApiService = bazonApiService;
        _unitOfWork = unitOfWork;
        _posterService = posterService;
    }

    public async Task LoadAsync(long id, CancellationToken token = default)
    {
        var data = await GetFilmAsync(id);
        await AddFilmAsync(data);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task AddFilmAsync(FilmData film)
    {
        var poster = await _posterService.SaveAsync(film.Film.PosterUrl!);
        var builder = FilmBuilder.Create()
            .WithName(film.Film.Title)
            .WithDescription(film.Film.Description!)
            .WithYear(film.Film.Year)
            .WithRating(film.Film.Rating ?? 0)
            .WithType(film.Film.Serial ? FilmType.Serial : FilmType.Film)
            .WithCdn(film.FilmCdns.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)))
            .WithGenres(film.Film.Genres)
            .WithActors(film.FilmStaff.Actors.Take(10).ToList())
            .WithDirectors(film.FilmStaff.Directors.Take(10).ToList())
            .WithScreenwriters(film.FilmStaff.ScreenWriters.Take(10).ToList())
            .WithCountries(film.Film.Countries)
            .WithPoster(poster);
        if (!string.IsNullOrEmpty(film.Film.ShortDescription))
            builder = builder.WithShortDescription(film.Film.ShortDescription);
        if (film.Film.Serial)
        {
            var releasedSeasons = film.FilmSeasons!.Where(x => x.Episodes.Any(e => e.ReleaseDate.HasValue)).ToList();
            builder = builder.WithEpisodes(releasedSeasons.Count, releasedSeasons.Sum(x => x.Episodes.Count));
        }
        await _unitOfWork.FilmRepository.Value.AddAsync(builder.Build());
    }

    private async Task<FilmData> GetFilmAsync(long kpId)
    {
        var film = await _kpApi.GetAsync(kpId);
        var staff = await _kpApi.GetActorsAsync(kpId);
        var seasons = film.Serial ? await _kpApi.GetSeasonsAsync(kpId) : null;
        var cdn = new List<Cdn>();
        try
        {
            cdn.Add(await _bazonApiService.GetInfoAsync(kpId));
            cdn.Add(await _videoCdnApi.GetInfoAsync(kpId));
        }
        catch
        {
            // ignored
        }

        return new FilmData(film, staff, seasons, cdn);
    }
}