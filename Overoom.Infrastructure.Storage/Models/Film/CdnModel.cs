using Overoom.Domain.Films.Enums;
using Overoom.Infrastructure.Storage.Models.Voice;

namespace Overoom.Infrastructure.Storage.Models.Film;

public class CdnModel
{
    public long Id { get; set; }
    public CdnType Type { get; set; }
    public Uri Uri { get; set; } = null!;
    public string Quality { get; set; } = null!;
    public List<VoiceModel> Voices { get; set; } = new();
    public FilmModel Film { get; set; } = null!;
}