using Newtonsoft.Json;

namespace Overoom.Infrastructure.Movie.Models;

public class Season
{
  [JsonProperty("number")] public int Number { get; set; }
  [JsonProperty("episodes")] public List<Episode> Episodes { get; set; } = null!;
}