using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class FilmData
{
    [JsonProperty("nameRu")] public string? NameRu { get; set; }
    [JsonProperty("nameEn")] public string? NameEn { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("kinopoiskId")] public long KpId { get; set; }
    [JsonProperty("imdbId")] public string? ImdbId { get; set; }
    [JsonProperty("serial")] public bool Serial { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("shortDescription")] public string? ShortDescription { get; set; }
    [JsonProperty("posterUrl")] public string? PosterUrl { get; set; }
    [JsonProperty("ratingKinopoisk")] public double? RatingKinopoisk { get; set; }
    [JsonProperty("ratingImdb")] public double? RatingImdb { get; set; }
    [JsonProperty("countries")] public List<string> Countries { get; set; } = null!;
    [JsonProperty("genres")] public List<string> Genres { get; set; } = null!;
}