using Newtonsoft.Json;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Movie.Abstractions;
using Overoom.Infrastructure.Movie.Converters;
using Overoom.Infrastructure.Movie.Models;

namespace Overoom.Infrastructure.Movie.Services;

public class BazonResponseParser : IBazonResponseParser
{
    private readonly JsonSerializerSettings _settings = new()
        { Converters = { new BazonConverter() } };

    public Cdn Get(string json)
    {
        var cdn = JsonConvert.DeserializeObject<List<Bazon>>(json, _settings)!;
        var uri = new Uri(cdn.First().Uri!);
        var quality = string.Join(", ", cdn.Select(x => x.Quality).Distinct());
        var voices = cdn.Select(x => x.Voice!).Distinct().ToList();
        return new Cdn(uri, quality, voices, CdnType.Bazon);
    }
}