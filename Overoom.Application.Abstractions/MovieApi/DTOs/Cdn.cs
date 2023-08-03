using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.MovieApi.DTOs;

public class Cdn
{
    public Cdn(Uri uri, string quality, IReadOnlyCollection<string> voices, CdnType type)
    {
        Uri = uri;
        Quality = quality;
        Voices = voices;
        Type = type;
    }
    public CdnType Type { get; }
    public Uri Uri { get; }
    public string Quality { get; }
    public IReadOnlyCollection<string> Voices { get; }
}