using Newtonsoft.Json;
using Overoom.Infrastructure.MovieDownloader.Enums;

namespace Overoom.Infrastructure.MovieDownloader.Models;

public class ActorsData
{
    [JsonProperty("nameRu")] public string? Name { get; set; }
    [JsonProperty("nameEn")] public string? NameEn { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("professionKey")] public Profession Profession { get; set; }
}