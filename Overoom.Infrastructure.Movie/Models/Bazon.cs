using Newtonsoft.Json;

namespace Overoom.Infrastructure.Movie.Models;

public class Bazon
{
    [JsonProperty("link")] public string? Uri { get; set; }
    [JsonProperty("quality")] public string? Quality { get; set; }
    [JsonProperty("translation")] public string? Voice  { get; set; }
}