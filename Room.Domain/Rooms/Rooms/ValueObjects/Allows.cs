namespace Room.Domain.Rooms.Rooms.ValueObjects;

/// <summary>
/// Класс Allows представляет собой набор разрешений.
/// </summary>
public class Allows
{
    /// <summary>
    /// Разрешение на совершение звукового сигнала.
    /// </summary>
    public required bool Beep { get; init; }

    /// <summary>
    /// Разрешение кричать.
    /// </summary>
    public required bool Scream { get; init; }

    /// <summary>
    /// Разрешение на изменение.
    /// </summary>
    public required bool Change { get; init; }
}
