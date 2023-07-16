using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.Film;

public class CdnViewModel
{
    public CdnViewModel(CdnType cdn, IEnumerable<string> voices, string quality)
    {
        Cdn = cdn;
        Quality = quality;
        Voices = string.Join(", ", voices);
    }

    public CdnType Cdn { get; }
    public string Quality { get; }
    public string Voices { get; }
}