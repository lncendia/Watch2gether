using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class FilmSearchResponse
{
    [JsonProperty("total")] public int Total { get; init; }
    [JsonProperty("totalPages")] public int TotalPages { get; init; }
    [JsonProperty("items")] public List<FilmShortData> Items { get; init; } = null!;
}