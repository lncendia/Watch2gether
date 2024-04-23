using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на удаление видео из списка
/// </summary>
public class RemoveVideoCommand : RoomCommand
{
    /// <summary>
    /// Идентификатор видео
    /// </summary>
    public required string VideoId { get; init; }
}