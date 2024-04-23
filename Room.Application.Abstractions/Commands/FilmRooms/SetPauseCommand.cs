using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на установку паузы
/// </summary>
public class SetPauseCommand : RoomCommand
{
    /// <summary>
    /// Флаг нахождения на паузе
    /// </summary>
    public required bool Pause { get; init; }
}