using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kodik.Models;

public class MaterialData
{
    [JsonProperty("title")] public string Title { get; init; }

    [JsonProperty("title_en")] public string TitleEn { get; init; }

    [JsonProperty("year")] public int Year { get; init; }

    [JsonProperty("tagline")] public string Tagline { get; init; }

    [JsonProperty("description")] public string Description { get; init; }

    [JsonProperty("poster_url")] public string PosterUrl { get; init; }

    [JsonProperty("countries")] public List<string> Countries { get; init; }

    [JsonProperty("genres")] public List<string> Genres { get; init; }

    [JsonProperty("kinopoisk_rating")] public float KinopoiskRating { get; init; }

    [JsonProperty("kinopoisk_votes")] public int KinopoiskVotes { get; init; }

    [JsonProperty("imdb_rating")] public float ImdbRating { get; init; }

    [JsonProperty("imdb_votes")] public int ImdbVotes { get; init; }

    [JsonProperty("actors")] public List<string> Actors { get; init; }

    [JsonProperty("directors")] public List<string> Directors { get; init; }

    [JsonProperty("producers")] public List<string> Producers { get; init; }

    [JsonProperty("writers")] public List<string> Writers { get; init; }

    [JsonProperty("composers")] public List<string> Composers { get; init; }

    [JsonProperty("editors")] public List<string> Editors { get; init; }

    [JsonProperty("designers")] public List<string> Designers { get; init; }

    [JsonProperty("operators")] public List<string> Operators { get; init; }
}