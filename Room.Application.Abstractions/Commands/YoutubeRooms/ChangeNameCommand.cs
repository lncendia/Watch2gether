using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на изменение имени другому пользователю
/// </summary>
public class ChangeNameCommand : RoomTargetCommand
{
    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; init; }
}