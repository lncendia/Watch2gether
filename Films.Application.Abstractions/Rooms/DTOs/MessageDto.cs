namespace Films.Application.Abstractions.Rooms.DTOs;

public class MessageDto
{
    public required string Username { get; init; }
    public required int ViewerId { get; init; }
    public required Uri AvatarUrl { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Text { get; init; }
}