using Room.Domain.BaseRoom.ValueObjects;

namespace Room.Application.Abstractions.Commands.BaseRooms;

public class ViewerData
{
    public required Guid Id { get; init; }
    public required string Nickname { get; init; }
    public required Uri PhotoUrl { get; init; }
    public required Allows Allows { get; init; }
}