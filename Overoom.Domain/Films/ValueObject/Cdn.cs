using Overoom.Domain.Films.Enums;
using Overoom.Domain.Films.Exceptions;

namespace Overoom.Domain.Films.ValueObject;

public class Cdn
{
    internal Cdn(CdnType type, Uri uri, string quality, List<string> voices)
    {
        Type = type;
        Uri = uri;
        Quality = quality;
        _voices = voices;
        if (!Voices.Any()) throw new EmptyVoicesCollectionException();
    }

    public CdnType Type { get; }
    public Uri Uri { get; }
    public string Quality { get; }

    private readonly List<string> _voices;
    public IReadOnlyCollection<string> Voices => _voices.AsReadOnly();
}