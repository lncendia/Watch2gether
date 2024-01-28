namespace Films.Application.Abstractions.Rooms.DTOs;

public class AllowsDto
{
    public required bool Beep { get; init; }
    public required bool Scream { get; init; }
    public required bool Change { get; init; }
}