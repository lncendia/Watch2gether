namespace Films.Application.Abstractions.Rooms.DTOs;

public abstract class ViewerDto
{
    public required int Id { get; init; }
    public required string Username { get; init; }
    public required Uri AvatarUrl { get; init; }
    public required bool Pause { get; init; }
    public required bool FullScreen { get; init; }
    public required TimeSpan Time { get; init; }
    public required AllowsDto Allows { get; init; }
}