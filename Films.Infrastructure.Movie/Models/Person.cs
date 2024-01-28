using Films.Infrastructure.Movie.Enums;
using Newtonsoft.Json;

namespace Films.Infrastructure.Movie.Models;

public class Person
{
    [JsonProperty("nameRu")] public string? Name { get; set; }
    [JsonProperty("nameEn")] public string? NameEn { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("professionKey")] public Profession Profession { get; set; }
}