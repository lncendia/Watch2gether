using Films.Domain.Films;
using Films.Domain.Rooms.BaseRoom.Events;

namespace Films.Domain.Rooms.FilmRooms.Events;

/// <summary>
/// Класс, представляющий событие создания новой комнаты с фильмом.
/// </summary>
public class FilmRoomCreatedDomainEvent : RoomCreatedDomainEvent<FilmRoom>
{
    public required Film Film { get; init; }
}