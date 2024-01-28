namespace Films.Application.Abstractions.Rooms.DTOs;

public abstract class RoomDto
{
    public required int OwnerId { get; init; }
    public required bool IsOpen { get; init; }
    public required IReadOnlyCollection<MessageDto> Messages { get; init; }
}