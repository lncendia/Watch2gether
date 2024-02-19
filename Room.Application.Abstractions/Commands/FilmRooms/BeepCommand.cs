using Room.Application.Abstractions.Commands.BaseRooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на отправку звукового сигнала другому пользователю
/// </summary>
public class BeepCommand : RoomTargetCommand;