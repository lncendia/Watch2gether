using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class SeasonsResponse
{
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("items")] public List<Season> Seasons { get; set; } = null!;
}