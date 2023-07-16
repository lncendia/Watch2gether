using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Movie.DTOs;

public class CdnDto
{
    public CdnDto(CdnType type, string quality, IReadOnlyCollection<string> voices)
    {
        Type = type;
        Quality = quality;
        Voices = voices;
    }

    public CdnType Type { get; }
    public string Quality { get; }
    public IReadOnlyCollection<string> Voices { get; }
}