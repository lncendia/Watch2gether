using Room.Application.Abstractions.Commands.Base;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на смену текущего видео
/// </summary>
public class ChangeVideoCommand : RoomCommand
{
    /// <summary>
    /// Номер видео
    /// </summary>
    public required int VideoNumber { get; init; }
}