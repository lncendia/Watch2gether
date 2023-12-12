using System.ComponentModel.DataAnnotations.Schema;
using Overoom.Infrastructure.Storage.Models.Film;

namespace Overoom.Infrastructure.Storage.Models.Person;

public class PersonModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}