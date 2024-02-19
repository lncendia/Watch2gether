using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на установку режима полного экрана
/// </summary>
public class SetFullscreenCommand : RoomCommand
{
    /// <summary>
    /// Флаг нахождения в полноэкранном режиме
    /// </summary>
    public required bool Fullscreen { get; init; }
}