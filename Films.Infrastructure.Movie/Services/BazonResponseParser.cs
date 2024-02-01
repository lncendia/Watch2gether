using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Newtonsoft.Json;
using Films.Infrastructure.Movie.Abstractions;
using Films.Infrastructure.Movie.Converters;
using Films.Infrastructure.Movie.Models;

namespace Films.Infrastructure.Movie.Services;

public class BazonResponseParser : IBazonResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new BazonConverter() } };

    public CdnApiResponse Get(string json)
    {
        var cdn = JsonConvert.DeserializeObject<List<Bazon>>(json, _settings)!;
        var uri = new Uri(cdn.First().Uri!);
        var quality = string.Join(", ", cdn.Select(x => x.Quality).Distinct());
        var voices = cdn.Select(x => x.Voice!).Distinct().ToArray();
        return new CdnApiResponse
        {
            Url = uri,
            Quality = quality,
            Voices = voices
        };
    }
}