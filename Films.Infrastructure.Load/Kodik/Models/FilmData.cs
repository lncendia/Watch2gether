using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kodik.Models;

public class FilmData
{
    [JsonProperty("type")] public string Type { get; init; } = null!;

    [JsonProperty("link")] public string Link { get; init; } = null!;

    [JsonProperty("title")] public string Title { get; init; } = null!;

    [JsonProperty("title_orig")] public string TitleOrig { get; init; } = null!;

    [JsonProperty("year")] public int Year { get; init; }

    [JsonProperty("last_season")] public int LastSeason { get; init; }

    [JsonProperty("last_episode")] public int LastEpisode { get; init; }

    [JsonProperty("episodes_count")] public int EpisodesCount { get; init; }

    [JsonProperty("kinopoisk_id")] public string? KinopoiskId { get; init; }

    [JsonProperty("imdb_id")] public string? ImdbId { get; init; }

    [JsonProperty("quality")] public string Quality { get; init; } = null!;

    [JsonProperty("material_data")] public MaterialData MaterialData { get; init; } = null!;
}