using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на смену текущего видео
/// </summary>
public class ChangeVideoCommand : RoomCommand
{
    /// <summary>
    /// Идентификатор видео
    /// </summary>
    public required string VideoId { get; init; }
}