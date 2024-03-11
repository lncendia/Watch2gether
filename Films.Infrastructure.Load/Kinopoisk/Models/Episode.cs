using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class Episode
{
    [JsonProperty("episodeNumber")] public int EpisodeNumber { get; init; }
    [JsonProperty("releaseDate")] public DateOnly? ReleaseDate { get; init; }
}