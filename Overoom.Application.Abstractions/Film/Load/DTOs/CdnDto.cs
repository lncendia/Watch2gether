using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Abstractions.Film.Load.DTOs;

public class CdnDto
{
    public CdnDto(CdnType type, Uri uri, string quality, IReadOnlyCollection<string> voices)
    {
        Type = type;
        Uri = uri;
        Quality = quality;
        Voices = voices;
    }

    public CdnType Type { get; }
    public Uri Uri { get; }
    public string Quality { get; }
    public IReadOnlyCollection<string> Voices { get; }
}