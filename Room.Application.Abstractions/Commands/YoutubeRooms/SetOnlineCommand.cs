using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на установку статуса подключения зрителя
/// </summary>
public class SetOnlineCommand : RoomTargetCommand
{
    public required bool Online { get; init; }
}