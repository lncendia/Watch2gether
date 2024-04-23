using System.ComponentModel.DataAnnotations;
using Films.Infrastructure.Storage.Models.Persons;

namespace Films.Infrastructure.Storage.Models.Films;

public class FilmActorModel
{
    public long Id { get; set; }
    public PersonModel Person { get; set; } = null!;
    public FilmModel Film { get; set; } = null!;
    [MaxLength(100)] public string? Description { get; set; }
}