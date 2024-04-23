using System.Net;
using Films.Infrastructure.Load.Exceptions;
using Films.Infrastructure.Load.Kinopoisk.Converters;
using Films.Infrastructure.Load.Kinopoisk.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Films.Infrastructure.Load.Kinopoisk;

public class KinopoiskApi : IKinopoiskApi, IDisposable
{
    private readonly RestClient _kpClient;

    private static readonly JsonSerializerSettings Settings = new()
        { Converters = { new ListObjectsConverter(), new ProfessionConverter() } };


    public KinopoiskApi(string kpToken)
    {
        _kpClient = new RestClient(options => options.MaxTimeout = 15000);
        _kpClient.AddDefaultHeader("X-API-KEY", kpToken);
    }

    public async Task<FilmData> GetAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}");
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<Person>> GetActorsAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v1/staff");
        request.AddQueryParameter("filmId", kpId);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<Person[]>(json, Settings)!;
    }

    public async Task<SeasonsResponse> GetSeasonsAsync(long kpId, CancellationToken token = default)
    {
        var request = new RestRequest($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{kpId}/seasons");
        request.AddQueryParameter("filmId", kpId);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<SeasonsResponse>(json, Settings)!;
    }

    public async Task<FilmSearchResponse> FindByTitleAsync(string title, int? year, int page = 1,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films");
        if (!string.IsNullOrEmpty(title)) request.AddQueryParameter("keyword", title);
        request.AddQueryParameter("order", "YEAR");
        request.AddQueryParameter("page", page.ToString());
        if (year != null)
        {
            request.AddQueryParameter("yearFrom", year.Value);
            request.AddQueryParameter("yearTo", year.Value);
        }

        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmSearchResponse>(json, Settings)!;
    }


    public async Task<FilmSearchResponse> FindByImdbAsync(string imdbId, CancellationToken token = default)
    {
        var request = new RestRequest("https://kinopoiskapiunofficial.tech/api/v2.2/films");
        request.AddQueryParameter("imdbId", imdbId);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmSearchResponse>(json, Settings)!;
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

    public void Dispose()
    {
        _kpClient.Dispose();
    }
}