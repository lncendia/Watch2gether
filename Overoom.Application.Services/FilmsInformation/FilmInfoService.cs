using Overoom.Application.Abstractions.FilmsInformation.DTOs;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Kinopoisk.Interfaces;

namespace Overoom.Application.Services.FilmsInformation;

public class FilmInfoService : IFilmInfoService
{
    private readonly IKpApiService _kpApi;
    private readonly IFilmKpMapper _mapper;
    private readonly IVideoCdnApiService _videoCdnApi;

    public FilmInfoService(IKpApiService kpApi, IFilmKpMapper mapper, IVideoCdnApiService videoCdnApi)
    {
        _kpApi = kpApi;
        _mapper = mapper;
        _videoCdnApi = videoCdnApi;
    }

    public async Task<FilmDto> GetFromTitleAsync(string title)
    {
        var filmShort = await _kpApi.GetFromTitleAsync(title);
        return await GetFilmAsync(filmShort.KpId);
    }

    public async Task<FilmDto> GetFromImdbAsync(string id)
    {
        var filmShort = await _kpApi.GetFromImdbAsync(id);
        return await GetFilmAsync(filmShort.KpId);
    }

    public Task<FilmDto> GetFromKpAsync(long id) => GetFilmAsync(id);


    private async Task<FilmDto> GetFilmAsync(long kpId)
    {
        var film = await _kpApi.GetFromKpAsync(kpId);
        var staff = await _kpApi.GetActorsAsync(kpId);
        var seasons = film.Serial ? await _kpApi.GetSeasonsAsync(kpId) : null;
        var cdn = new List<Cdn>();
        try
        {
            cdn.Add(await _videoCdnApi.GetInfoAsync(kpId));
        }
        catch
        {
            // ignored
        }

        return _mapper.Map(film, staff, seasons, cdn);
    }
}