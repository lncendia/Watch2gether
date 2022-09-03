using Newtonsoft.Json;

namespace Watch2gether.Infrastructure.MovieDownloader.Models;

public class FilmFullData
{
    [JsonProperty("description")] public string? Description { get; set; } = null!;
    [JsonProperty("shortDescription")] public string? ShortDescription { get; set; } = null!;
    [JsonProperty("posterUrl")] public string PosterUrl { get; set; } = null!;
    [JsonProperty("ratingKinopoisk")] public double? RatingKinopoisk { get; set; }
    [JsonProperty("ratingImdb")] public double? RatingImdb { get; set; }
    [JsonProperty("countries")] public List<string> Countries { get; set; } = null!;
    [JsonProperty("genres")] public List<string> Genres { get; set; } = null!;
}