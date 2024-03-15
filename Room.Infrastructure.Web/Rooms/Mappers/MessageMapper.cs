using Room.Application.Abstractions.DTOs.Messages;
using Room.Infrastructure.Web.Rooms.ViewModels;
using Room.Infrastructure.Web.Rooms.ViewModels.Messages;

namespace Room.Infrastructure.Web.Rooms.Mappers;

public class MessageMapper
{
    public static MessageViewModel Map(MessageDto dto) => new()
    {
        Id = dto.Id,
        UserId = dto.UserId,
        Text = dto.Text,
        CreatedAt = dto.CreatedAt
    };
}