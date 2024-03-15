using Room.Application.Abstractions.DTOs.Messages;
using Room.Domain.Messages.Messages;

namespace Room.Application.Services.Mappers;

/// <summary>
/// Статический класс для преобразования сообщения в DTO
/// </summary>
public static class MessageMapper
{
    public static MessageDto Map(Message message)
    {
        return new MessageDto
        {
            UserId = message.UserId,
            RoomId = message.RoomId,
            Text = message.Text,
            Id = message.Id,
            CreatedAt = message.CreatedAt
        };
    }
}