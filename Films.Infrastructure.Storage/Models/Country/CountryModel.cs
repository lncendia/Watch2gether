using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Storage.Models.Country;

public class CountryModel
{
    public long Id { get; set; }
    [MaxLength(20)] public string Name { get; set; } = null!;
}