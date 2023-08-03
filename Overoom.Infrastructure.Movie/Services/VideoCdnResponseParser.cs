using Newtonsoft.Json;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.Movie.Converters;

namespace Overoom.Infrastructure.Movie.Services;

public class VideoCdnResponseParser : IVideoCdnResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new VideoCdnConverter() } };

    public Cdn Get(string json)
    {
        var cdn = JsonConvert.DeserializeObject<Models.VideoCdn>(json, _settings)!;
        return new Cdn(new Uri("https:" + cdn.Uri), "BD", cdn.Voices.Where(x => !string.IsNullOrEmpty(x)).ToList(),
            CdnType.VideoCdn);
    }
}