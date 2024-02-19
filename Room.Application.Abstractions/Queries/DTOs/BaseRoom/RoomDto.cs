using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Application.Abstractions.Queries.DTOs.BaseRoom;

public abstract class RoomDto<T> where T : ViewerDto
{
    public required Guid Id { get; init; }
    public required Guid OwnerId { get; init; }
    public required IReadOnlyCollection<Message> Messages { get; init; }
    public required IReadOnlyCollection<T> Viewers { get; init; }
}