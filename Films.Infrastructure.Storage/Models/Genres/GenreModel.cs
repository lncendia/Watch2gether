using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Storage.Models.Genres;

public class GenreModel
{
    public long Id { get; set; }
    [MaxLength(30)] public string Name { get; set; } = null!;
}