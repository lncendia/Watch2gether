using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Storage.Models.Film;

public class CdnModel
{
    public long Id { get; set; }
    [MaxLength(30)] public string Name { get; set; } = null!;
    public Uri Url { get; set; } = null!;
    [MaxLength(30)] public string Quality { get; set; } = null!;
    public FilmModel Film { get; set; } = null!;
}