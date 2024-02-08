using Room.Application.Abstractions.Commands.Base;

namespace Room.Application.Abstractions.Commands.FilmRooms;

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