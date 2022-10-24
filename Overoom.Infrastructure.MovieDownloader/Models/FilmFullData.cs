using Newtonsoft.Json;

namespace Overoom.Infrastructure.MovieDownloader.Models;

public class FilmFullData
{
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("shortDescription")] public string? ShortDescription { get; set; }
    [JsonProperty("posterUrl")] public string PosterUrl { get; set; } = null!;
    [JsonProperty("ratingKinopoisk")] public double? RatingKinopoisk { get; set; }
    [JsonProperty("ratingImdb")] public double? RatingImdb { get; set; }
    [JsonProperty("countries")] public List<string> Countries { get; set; } = null!;
    [JsonProperty("genres")] public List<string> Genres { get; set; } = null!;
}