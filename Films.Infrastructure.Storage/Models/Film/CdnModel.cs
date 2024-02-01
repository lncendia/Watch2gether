using Films.Domain.Films.Enums;
using Films.Infrastructure.Storage.Models.Voice;

namespace Films.Infrastructure.Storage.Models.Film;

public class CdnModel
{
    public long Id { get; set; }
    public CdnType Type { get; set; }
    public Uri Url { get; set; } = null!;
    public string Quality { get; set; } = null!;
    public List<VoiceModel> Voices { get; set; } = [];
    public FilmModel Film { get; set; } = null!;
}