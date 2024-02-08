using Room.Domain.Rooms.BaseRoom.ValueObjects;

namespace Room.Application.Abstractions.DTOs.Base;

public abstract class RoomDto<T> where T : ViewerDto
{
    public required Guid OwnerId { get; init; }
    public required string? Code { get; init; }
    public required IReadOnlyCollection<Message> Messages { get; init; }
    public required IReadOnlyCollection<T> Viewers { get; init; }
}