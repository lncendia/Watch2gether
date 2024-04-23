using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на установку таймлайна
/// </summary>
public class SetTimeLineCommand : RoomCommand
{
    /// <summary>
    /// Таймлайн
    /// </summary>
    public required TimeSpan TimeLine { get; init; }
}