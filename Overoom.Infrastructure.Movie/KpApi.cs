using System.Net;
using Overoom.Application.Abstractions.Film.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Film.Kinopoisk.Exceptions;
using Overoom.Application.Abstractions.Film.Kinopoisk.Interfaces;
using Overoom.Infrastructure.Movie.Abstractions;
using RestSharp;

namespace Overoom.Infrastructure.Movie;

public class KpApi : IKpApiService
{
    private readonly RestClient _kpClient;
    private readonly IResponseParser _parser;

    public KpApi(string kpToken, IResponseParser parser, HttpClient? client = null)
    {
        _parser = parser;
        _kpClient = client != null ? new RestClient(client, true) : new RestClient();
        _kpClient.AddDefaultHeader("X-API-KEY", kpToken);
    }

    public async Task<FilmShort> GetFromTitleAsync(string title)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films");
        request.AddQueryParameter("keyword", title);
        return _parser.GetFirstFilmFromSearch(await MakeRequestAsync(request));
    }

    public async Task<FilmShort> GetFromImdbAsync(string imdbId)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films");
        request.AddQueryParameter("imdbId", imdbId);
        return _parser.GetFirstFilmFromSearch(await MakeRequestAsync(request));
    }

    public async Task<Film> GetFromKpAsync(long kpId)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}");
        var response = await _kpClient.GetAsync(request);
        return _parser.GetFilm(await MakeRequestAsync(request));
    }

    public async Task<FilmStaff> GetActorsAsync(long kpId)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v1/staff");
        request.AddQueryParameter("filmId", kpId);
        return _parser.GetStaff(await MakeRequestAsync(request));
    }

    public async Task<IReadOnlyCollection<Season>> GetSeasonsAsync(long kpId)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}/seasons");
        request.AddQueryParameter("filmId", kpId);
        return _parser.GetSeasons(await MakeRequestAsync(request));
    }

    private async Task<string> MakeRequestAsync(RestRequest request)
    {
        var response = await _kpClient.GetAsync(request);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return response.Content!;
    }
}