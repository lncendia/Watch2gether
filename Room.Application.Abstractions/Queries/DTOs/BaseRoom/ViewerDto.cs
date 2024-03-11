using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Application.Abstractions.Queries.DTOs.BaseRoom;

public abstract class ViewerDto
{
    public required Guid Id { get; init; }
    public required string Nickname { get; init; }
    public Uri? PhotoUrl { get; init; }
    public required bool Pause { get; init; }
    public required bool FullScreen { get; init; }

    public required bool Online { get; init; }
    public required TimeSpan TimeLine { get; init; }
    public required Allows Allows { get; init; }
}