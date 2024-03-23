using MediatR;
using Room.Application.Abstractions.DTOs.Messages;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на отправку сообщения
/// </summary>
public class SendMessageCommand : IRequest<MessageDto>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid ViewerId { get; init; }
    
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public required string Message { get; init; }
}