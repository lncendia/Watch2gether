using System.Net;
using Films.Infrastructure.Load.Exceptions;
using Films.Infrastructure.Load.Kodik.Converters;
using Films.Infrastructure.Load.Kodik.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Films.Infrastructure.Load.Kodik;

public class KodikApi : IKodikApi, IDisposable
{
    private readonly RestClient _restClient;

    private static readonly JsonSerializerSettings Settings = new()
        { Converters = { new KodikConverter() } };

    public KodikApi(string cdnToken)
    {
        _restClient = new RestClient(options => options.MaxTimeout = 15000);
        _restClient.AddDefaultQueryParameter("token", cdnToken);
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByTitleAsync(string title, bool includeMaterials, int? year,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("title", title);
        request.AddQueryParameter("full_match", true);
        request.AddQueryParameter("with_material_data", includeMaterials);
        if (year.HasValue) request.AddQueryParameter("year", year.Value);
        request.AddQueryParameter("limit", "10");
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByKinopoiskIdAsync(long kpId, bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("kinopoisk_id", kpId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByImdbIdAsync(string imdbId, bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("imdb_id", imdbId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByMdlIdAsync(long mdlId, bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("mdl_id", mdlId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByWorldArtAnimationIdAsync(long worldArtAnimationId,
        bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("worldart_animation_id", worldArtAnimationId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByWorldArtCinemaIdAsync(long worldArtCinemaId,
        bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("worldart_cinema_id", worldArtCinemaId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByWorldArtLinkAsync(string worldArtLink, bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("worldart_link", worldArtLink);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    public async Task<IReadOnlyCollection<FilmData>> GetByShikimoriIdAsync(long shikimoriId, bool includeMaterials,
        CancellationToken token = default)
    {
        var request = new RestRequest("https://kodikapi.com/search");
        request.AddQueryParameter("kinopoisk_id", shikimoriId);
        request.AddQueryParameter("with_material_data", includeMaterials);
        var json = await MakeRequestAsync(request, token);
        return JsonConvert.DeserializeObject<FilmData[]>(json, Settings)!;
    }

    private async Task<string> MakeRequestAsync(RestRequest request, CancellationToken token)
    {
        var response = await _restClient.GetAsync(request, cancellationToken: token);
        if (response.StatusCode == HttpStatusCode.NotFound) throw new ApiNotFoundException();
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ApiException($"The request was executed with the code {(int)response.StatusCode}",
                response.ErrorException);
        return response.Content!;
    }

    public void Dispose()
    {
        _restClient.Dispose();
    }
}