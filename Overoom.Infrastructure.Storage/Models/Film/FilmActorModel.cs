using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Infrastructure.Storage.Models.Person;

namespace Overoom.Infrastructure.Storage.Models.Film;

public class FilmActorModel
{
    public long Id { get; set; }
    public PersonModel Person { get; set; } = null!;
    public FilmModel Film { get; set; } = null!;
    public string? Description { get; set; }
}