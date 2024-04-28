using Films.Domain.Extensions;

namespace Films.Domain.Films.ValueObjects;

/// <summary>
/// Класс, представляющий информацию о домене сервиса доставки контента (CDN).
/// </summary>
public class Cdn
{
    private const int MaxNameLength = 30;

    private readonly string _name = null!;

    /// <summary>
    /// Название сервиса доставки контента.
    /// </summary>
    public required string Name
    {
        get => _name;
        init => _name = value.ValidateLength(MaxNameLength);
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