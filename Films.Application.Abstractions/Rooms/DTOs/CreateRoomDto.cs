namespace Films.Application.Abstractions.Rooms.DTOs;

public abstract class CreateRoomDto
{
    public required bool IsOpen { get; init; }
}