using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на установку статуса подключения зрителя
/// </summary>
public class SetOnlineCommand : RoomTargetCommand
{
    public required bool Online { get; init; }
}