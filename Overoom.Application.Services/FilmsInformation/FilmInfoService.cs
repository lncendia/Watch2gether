using Overoom.Application.Abstractions.FilmsInformation.DTOs;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Application.Abstractions.MovieApi.Interfaces;

namespace Overoom.Application.Services.FilmsInformation;

public class FilmInfoService : IFilmInfoService
{
    private readonly IKpApiService _kpApi;
    private readonly IFilmKpMapper _mapper;
    private readonly IVideoCdnApiService _videoCdnApi;
    private readonly IBazonApiService _bazonApiService;

    public FilmInfoService(IKpApiService kpApi, IFilmKpMapper mapper, IVideoCdnApiService videoCdnApi,
        IBazonApiService bazonApiService)
    {
        _kpApi = kpApi;
        _mapper = mapper;
        _videoCdnApi = videoCdnApi;
        _bazonApiService = bazonApiService;
    }

    public async Task<FilmDto> GetFromTitleAsync(string title)
    {
        var filmShort = await _kpApi.FindAsync(title, null);
        return await GetFilmAsync(filmShort.First().KpId);
    }

    public async Task<FilmDto> GetFromImdbAsync(string id)
    {
        var filmShort = await _kpApi.FindAsync(null, id);
        return await GetFilmAsync(filmShort.First().KpId);
    }

    public Task<FilmDto> GetFromKpAsync(long id) => GetFilmAsync(id);


    private async Task<FilmDto> GetFilmAsync(long kpId)
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

        return _mapper.Map(film, staff, seasons, cdn);
    }
}