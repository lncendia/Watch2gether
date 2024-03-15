namespace Room.Application.Abstractions.DTOs.Rooms;

public abstract class RoomDto<T> where T : ViewerDto
{
    public required Guid Id { get; init; }
    public required Guid OwnerId { get; init; }
    public required IReadOnlyCollection<T> Viewers { get; init; }
}