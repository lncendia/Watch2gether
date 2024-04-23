using Films.Infrastructure.Load.Kinopoisk.Enums;
using Newtonsoft.Json;

namespace Films.Infrastructure.Load.Kinopoisk.Models;

public class Person
{
    [JsonProperty("nameRu")] public string? Name { get; init; }
    [JsonProperty("nameEn")] public string? NameEn { get; init; }
    [JsonProperty("description")] public string? Description { get; init; }
    [JsonProperty("professionKey")] public Profession Profession { get; init; }
}