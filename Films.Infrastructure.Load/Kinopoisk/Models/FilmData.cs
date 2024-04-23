using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class FilmData
{
    [JsonProperty("nameRu")] public string? NameRu { get; init; }
    [JsonProperty("nameEn")] public string? NameEn { get; init; }
    [JsonProperty("year")] public int Year { get; init; }
    [JsonProperty("kinopoiskId")] public long KpId { get; init; }
    [JsonProperty("imdbId")] public string? ImdbId { get; init; }
    [JsonProperty("serial")] public bool Serial { get; init; }
    [JsonProperty("description")] public string? Description { get; init; }
    [JsonProperty("shortDescription")] public string? ShortDescription { get; init; }
    [JsonProperty("posterUrl")] public string? PosterUrl { get; init; }
    [JsonProperty("ratingKinopoisk")] public double? RatingKinopoisk { get; init; }
    [JsonProperty("ratingImdb")] public double? RatingImdb { get; init; }
    [JsonProperty("countries")] public List<string> Countries { get; init; } = null!;
    [JsonProperty("genres")] public List<string> Genres { get; init; } = null!;
}