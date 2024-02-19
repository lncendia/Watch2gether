using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

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