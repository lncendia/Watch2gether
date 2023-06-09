using Overoom.Domain.Film.Enums;

namespace Overoom.Infrastructure.Storage.Models.Films;

public class CdnModel
{
    public long Id { get; set; }
    public CdnType Type { get; set; }
    public Uri Uri { get; set; } = null!;
    public string Quality { get; set; } = null!;
    public List<VoiceModel> Voices { get; set; } = new();
}