using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на отправку звукового сигнала другому пользователю
/// </summary>
public class BeepCommand : RoomTargetCommand;