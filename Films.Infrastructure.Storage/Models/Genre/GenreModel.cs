using System.ComponentModel.DataAnnotations.Schema;

namespace Films.Infrastructure.Storage.Models.Genre;

public class GenreModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}