using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Room.Domain.Films.Enums;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Models.Film;

public class FilmModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public FilmType Type { get; set; }
    [MaxLength(200)] public string Title { get; set; } = null!;
    public Uri PosterUrl { get; set; } = null!;
    [MaxLength(1500)] public string Description { get; set; } = null!;
    public int Year { get; set; }

    public List<CdnModel> CdnList { get; set; } = [];
}