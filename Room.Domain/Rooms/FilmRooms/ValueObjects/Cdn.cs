using Room.Domain.Extensions;

namespace Room.Domain.Rooms.FilmRooms.ValueObjects;

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
        init => _name = value.ValidateLength(30);
    }

    /// <summary>
    /// URL-адрес сервиса доставки контента.
    /// </summary>
    public required Uri Url { get; init; }
}