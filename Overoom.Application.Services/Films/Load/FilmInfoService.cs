using Overoom.Application.Abstractions.Films.Kinopoisk.Interfaces;
using Overoom.Application.Abstractions.Films.Load.DTOs;
using Overoom.Application.Abstractions.Films.Load.Interfaces;

namespace Overoom.Application.Services.Films.Load;

public class FilmInfoService : IFilmInfoService
{
    private readonly IKpApiService _kpApi;
    private readonly IFilmKpMapper _mapper;

    public FilmInfoService(IKpApiService kpApi, IFilmKpMapper mapper)
    {
        _kpApi = kpApi;
        _mapper = mapper;
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
        return _mapper.Map(film, staff, seasons);
    }
}