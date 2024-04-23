using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на добавление видео в очередь
/// </summary>
public class AddVideoCommand : RoomCommand
{
    /// <summary>
    /// Ссылка на видео
    /// </summary>
    public required Uri VideoUrl { get; init; }
}