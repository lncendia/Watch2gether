using Room.Application.Abstractions.Commands.Rooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на исключение пользователя из комнаты
/// </summary>
public class KickCommand : RoomTargetCommand;