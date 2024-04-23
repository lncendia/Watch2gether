using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на установку статуса подключения зрителя
/// </summary>
public class SetOnlineCommand : RoomCommand
{
    public required bool Online { get; init; }
}