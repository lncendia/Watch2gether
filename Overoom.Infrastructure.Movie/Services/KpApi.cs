using System.Net;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Application.Abstractions.MovieApi.Exceptions;
using Overoom.Application.Abstractions.MovieApi.Interfaces;
using Overoom.Infrastructure.Movie.Abstractions;
using RestSharp;

namespace Overoom.Infrastructure.Movie.Services;

public class KpApi : IKpApiService
{
    private readonly RestClient _kpClient;
    private readonly IKpResponseParser _parser;

    public KpApi(string kpToken, IKpResponseParser parser, HttpClient? client = null)
    {
        _parser = parser;
        _kpClient = client != null ? new RestClient(client, true) : new RestClient();
        _kpClient.AddDefaultHeader("X-API-KEY", kpToken);
    }

    public async Task<Film> GetAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}");
        return _parser.GetFilm(await MakeRequestAsync(request, token));
    }

    public async Task<FilmStaff> GetActorsAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v1/staff");
        request.AddQueryParameter("filmId", kpId);
        return _parser.GetStaff(await MakeRequestAsync(request, token));
    }

    public async Task<IReadOnlyCollection<Season>> GetSeasonsAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}/seasons");
        request.AddQueryParameter("filmId", kpId);
        return _parser.GetSeasons(await MakeRequestAsync(request, token));
    }

    public async Task<IReadOnlyCollection<FilmShort>> FindAsync(string? title, string? imdbId, int page = 1,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films");
        if (!string.IsNullOrEmpty(title)) request.AddQueryParameter("keyword", title);
        if (!string.IsNullOrEmpty(imdbId)) request.AddQueryParameter("imdbId", imdbId);
        request.AddQueryParameter("order", "YEAR");
        request.AddQueryParameter("ratingFrom", "5");
        request.AddQueryParameter("yearTo", DateTime.UtcNow.Year);
        request.AddQueryParameter("page", page.ToString());
        return _parser.GetFilms(await MakeRequestAsync(request, token));
    }

    private async Task<string> MakeRequestAsync(RestRequest request, CancellationToken token)
    {
        var response = await _kpClient.GetAsync(request, cancellationToken: token);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return response.Content!;
    }
}