using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class FilmSearchResponse
{
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("totalPages")] public int TotalPages { get; set; }
    [JsonProperty("items")] public List<FilmShortData> Items { get; set; } = null!;
}