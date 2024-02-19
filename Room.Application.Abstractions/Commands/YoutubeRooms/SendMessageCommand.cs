using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на отправку сообщения
/// </summary>
public class SendMessageCommand : RoomCommand
{
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public required string Message { get; init; }
}