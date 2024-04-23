using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Storage.Models.Persons;

public class PersonModel
{
    public long Id { get; set; }
    [MaxLength(50)] public string Name { get; set; } = null!;
}