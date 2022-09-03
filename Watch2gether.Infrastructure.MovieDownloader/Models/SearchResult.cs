using Newtonsoft.Json;
using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Infrastructure.MovieDownloader.Models;

public class SearchResult
{
    [JsonProperty("result")] public bool Success { get; set; }
    [JsonProperty("data")] public List<FilmData> Films { get; set; } = null!;
    [JsonProperty("current_page")] public int Page { get; set; }
    [JsonProperty("total")] public int Count { get; set; }
    [JsonProperty("last_page")] public int LastPage { get; set; }
}

public class FilmData
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; } = null!;
    [JsonProperty("kp_id")] public string? KpId { get; set; }
    [JsonProperty("type")] public FilmType FilmType { get; set; }
    [JsonProperty("year")] public DateTime DateTime { get; set; }
    [JsonProperty("seasons_count")] public int? SeasonsCount { get; set; }
    [JsonProperty("episodes_count")] public int? EpisodesCount { get; set; }
    [JsonProperty("iframe_src")] public string Url { get; set; } = null!;
}