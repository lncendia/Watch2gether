using System.ComponentModel.DataAnnotations.Schema;

namespace Films.Infrastructure.Storage.Models.Country;

public class CountryModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}