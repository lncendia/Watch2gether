using Films.Domain.Films.Exceptions;
using Room.Domain.Extensions;

namespace Room.Domain.Films.ValueObjects;

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
}