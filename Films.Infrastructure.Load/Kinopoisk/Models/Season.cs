using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class Season
{
  [JsonProperty("number")] public int Number { get; init; }
  [JsonProperty("episodes")] public List<Episode> Episodes { get; init; } = null!;
}