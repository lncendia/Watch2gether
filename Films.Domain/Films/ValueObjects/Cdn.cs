using Films.Domain.Extensions;
using Films.Domain.Films.Exceptions;

namespace Films.Domain.Films.ValueObjects;

/// <summary>
/// Класс, представляющий информацию о домене сервиса доставки контента (CDN).
/// </summary>
public class Cdn
{
    private readonly string _name = null!;

    /// <summary>
    /// Название сервиса доставки контента.
    /// </summary>
    public required string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 30) throw new CdnNameLengthException();
            _name = value.GetUpper();
        }
    }

    /// <summary>
    /// URL-адрес сервиса доставки контента.
    /// </summary>
    public required Uri Url { get; init; }
    
    /// <summary>
    /// Качество контента.
    /// </summary>
    public required string Quality { get; init; }
}