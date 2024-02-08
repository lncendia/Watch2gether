using Films.Domain.Extensions;
using Films.Domain.Films.Exceptions;

namespace Films.Domain.Films.ValueObjects;

public class Cdn
{
    private readonly string _name = null!;

    public required string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 30) throw new CdnNameLengthException();
            _name = value.GetUpper();
        }
    }

    public required Uri Url { get; init; }
    public required string Quality { get; init; }

    private readonly string[] _voices = null!;

    public required IReadOnlyCollection<string> Voices
    {
        get => _voices.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyVoicesCollectionException();
            if (value.Any(voice => string.IsNullOrEmpty(voice) || voice.Length > 30))
                throw new CdnVoiceLengthException();
            _voices = value.Select(s => s.GetUpper()).ToArray();
        }
    }
}