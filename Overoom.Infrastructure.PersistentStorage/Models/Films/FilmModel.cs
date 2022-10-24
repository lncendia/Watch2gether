using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Overoom.Domain.Films.Enums;

namespace Overoom.Infrastructure.PersistentStorage.Models.Films;

public class FilmModel
{
    public Guid Id { get; set; }
    public FilmType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string PosterFileName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public DateOnly Date { get; set; }
    public double Rating { get; set; }

    public int? CountSeasons { get; set; }
    public int? CountEpisodes { get; set; }


    public List<GenreModel> Genres { get; set; } = new();
    public List<CountryModel> Countries { get; set; } = new();
    public List<ActorModel> Actors { get; set; } = new();
    public List<DirectorModel> Directors { get; set; } = new();
    public List<ScreenWriterModel> ScreenWriters { get; set; } = new();
}