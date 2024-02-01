using Films.Domain.Films.Enums;
using Films.Domain.Films.Exceptions;

namespace Films.Domain.Films.ValueObjects;

public class Cdn
{
    public required CdnType Type { get; init; }
    public required Uri Url { get; init; }
    public required string Quality { get; init; }

    private readonly string[] _voices = null!;

    public required IReadOnlyCollection<string> Voices
    {
        get => _voices.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyVoicesCollectionException();
            _voices = value.ToArray();
        }
    }
}