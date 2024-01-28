using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class Episode
{
    [JsonProperty("episodeNumber")] public int EpisodeNumber { get; set; }
    [JsonProperty("releaseDate")] public DateOnly? ReleaseDate { get; set; }
}