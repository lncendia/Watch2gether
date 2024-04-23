using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class SeasonsResponse
{
    [JsonProperty("total")] public int Total { get; init; }
    [JsonProperty("items")] public List<Season> Seasons { get; init; } = null!;
}