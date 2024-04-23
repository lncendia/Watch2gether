using Room.Domain.Rooms.Rooms.ValueObjects;

namespace Room.Application.Abstractions.Commands.Rooms;

public class ViewerData
{
    public required Guid Id { get; init; }
    public required string Nickname { get; init; }
    public Uri? PhotoUrl { get; init; }
    public required Allows Allows { get; init; }
}