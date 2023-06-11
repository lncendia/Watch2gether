using Newtonsoft.Json;

namespace Overoom.Infrastructure.Movie.Models;

public class Episode
{
    [JsonProperty("episodeNumber")] public int EpisodeNumber { get; set; }
    [JsonProperty("nameRu")] public string? NameRu { get; set; }
}