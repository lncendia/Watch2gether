namespace Films.Application.Abstractions.Commands.Rooms.DTOs;

public class RoomServerDto
{
    public required Uri ServerUrl { get; init; }
    public required Guid Id { get; init; }
    
    public string? Code { get; init; }
}