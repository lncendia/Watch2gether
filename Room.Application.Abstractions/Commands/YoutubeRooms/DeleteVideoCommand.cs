using Room.Application.Abstractions.Commands.Base;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на удаление видео из списка
/// </summary>
public class DeleteVideoCommand : RoomCommand
{
    /// <summary>
    /// Номер видео
    /// </summary>
    public required int VideoNumber { get; init; }
}