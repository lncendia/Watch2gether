namespace Films.Application.Abstractions.DTOs.Rooms;

public class RoomServerDto
{
    public required Uri ServerUrl { get; init; }
    public required Guid Id { get; init; }
    
    public string? Code { get; init; }
}