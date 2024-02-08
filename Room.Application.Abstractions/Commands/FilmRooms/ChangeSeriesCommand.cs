using Room.Application.Abstractions.Commands.Base;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на изменение номера сезона и серии
/// </summary>
public class ChangeSeriesCommand : RoomCommand
{
    /// <summary>
    /// Сезон
    /// </summary>
    public required int Season { get; init; }

    /// <summary>
    /// Серия
    /// </summary>
    public required int Series { get; init; }
}