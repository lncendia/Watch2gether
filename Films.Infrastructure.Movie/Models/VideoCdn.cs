using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class VideoCdn
{
    [JsonProperty("iframe_src")] public string? Uri { get; set; }
    [JsonProperty("translations")] public List<string> Voices { get; set; } = null!;
}