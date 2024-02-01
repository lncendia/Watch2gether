using Films.Application.Abstractions.Services.MovieApi.DTOs;
using Newtonsoft.Json;
using Films.Infrastructure.Movie.Abstractions;
using Films.Infrastructure.Movie.Converters;

namespace Films.Infrastructure.Movie.Services;

public class VideoCdnResponseParser : IVideoCdnResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new VideoCdnConverter() } };

    public CdnApiResponse Get(string json)
    {
        var cdn = JsonConvert.DeserializeObject<Models.VideoCdn>(json, _settings)!;
        return new CdnApiResponse
        {
            Url = new Uri("https:" + cdn.Uri),
            Quality = "BD",
            Voices = cdn.Voices.Where(x => !string.IsNullOrEmpty(x)).ToArray()
        };
    }
}