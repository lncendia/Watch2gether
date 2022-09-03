using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Infrastructure.PersistentStorage.Models;

public class FilmModel
{
    public Guid Id { get; set; }
    public FilmType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string PosterFileName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }

    public int? CountSeasons { get; set; }
    public int? CountEpisodes { get; set; }

    public string GenresList { get; set; } = string.Empty;
    public string CountriesList { get; set; } = string.Empty;
    public string ActorsList { get; set; } = string.Empty;
    public string DirectorsList { get; set; } = string.Empty;
    public string ScreenWritersList { get; set; } = string.Empty;
    
    [NotMapped]
    public List<(string, string)> Actors
    {
        get => JsonSerializer.Deserialize<List<(string, string)>>(ActorsList, new JsonSerializerOptions {IncludeFields = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<(string, string)>();
        set => ActorsList = JsonSerializer.Serialize(value, new JsonSerializerOptions {IncludeFields = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [NotMapped]
    public List<string> Genres
    {
        get => JsonSerializer.Deserialize<List<string>>(GenresList, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<string>();
        set => GenresList = JsonSerializer.Serialize(value, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [NotMapped]
    public List<string> Countries
    {
        get => JsonSerializer.Deserialize<List<string>>(CountriesList, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<string>();
        set => CountriesList = JsonSerializer.Serialize(value, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [NotMapped]
    public List<string> Directors
    {
        get => JsonSerializer.Deserialize<List<string>>(DirectorsList, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<string>();
        set => DirectorsList = JsonSerializer.Serialize(value, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }

    [NotMapped]
    public List<string> ScreenWriters
    {
        get => JsonSerializer.Deserialize<List<string>>(ScreenWritersList, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}) ?? new List<string>();
        set => ScreenWritersList = JsonSerializer.Serialize(value, new JsonSerializerOptions {Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
    }
}