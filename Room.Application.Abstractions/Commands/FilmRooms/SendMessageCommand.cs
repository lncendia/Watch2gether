using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

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