namespace Room.Application.Abstractions.DTOs.Messages;

public class MessageDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid RoomId { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
}