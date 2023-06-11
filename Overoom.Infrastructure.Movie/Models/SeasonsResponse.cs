using Newtonsoft.Json;

namespace Overoom.Infrastructure.Movie.Models;

public class SeasonsResponse
{
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("items")] public List<Season> Seasons { get; set; } = null!;
}