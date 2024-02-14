using System.ComponentModel.DataAnnotations;

namespace Room.Infrastructure.Storage.Models.Film;

public class CdnModel
{
    [Key] public long Id { get; set; }

    [MaxLength(30)] public string Name { get; set; } = null!;
    public Uri Url { get; set; } = null!;

    public FilmModel Film { get; set; } = null!;
}