namespace Films.Application.Abstractions.DTOs.Rooms;

public abstract class RoomDto
{
    public required Guid Id { get; init; }
    public required int ViewersCount { get; init; }
    public required Uri ServerUrl { get; init; }
    
    public required bool IsClosed { get; init; }
}