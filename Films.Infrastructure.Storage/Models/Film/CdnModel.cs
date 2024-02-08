using System.ComponentModel.DataAnnotations;
using Films.Infrastructure.Storage.Models.Voice;

namespace Films.Infrastructure.Storage.Models.Film;

public class CdnModel
{
    public long Id { get; set; }
    [MaxLength(30)] public string Name { get; set; } = null!;
    public Uri Url { get; set; } = null!;
    [MaxLength(30)] public string Quality { get; set; } = null!;
    public List<VoiceModel> Voices { get; set; } = [];
    public FilmModel Film { get; set; } = null!;
}