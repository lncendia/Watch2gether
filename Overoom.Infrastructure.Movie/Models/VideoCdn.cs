using Newtonsoft.Json;

namespace Overoom.Infrastructure.Movie.Models;

public class VideoCdn
{
    [JsonProperty("iframe_src")] public string? Uri { get; set; }
    [JsonProperty("translations")] public List<string> Voices { get; set; } = null!;
}