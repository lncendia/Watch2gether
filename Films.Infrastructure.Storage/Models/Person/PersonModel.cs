using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Film;

namespace Films.Infrastructure.Storage.Models.Person;

public class PersonModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}