using Films.Domain.Rooms.BaseRoom.Events;

namespace Films.Domain.Rooms.YoutubeRooms.Events;

/// <summary>
/// Класс, представляющий событие создания новой комнаты ютуб.
/// </summary>
public class YoutubeRoomCreatedDomainEvent : RoomCreatedDomainEvent<YoutubeRoom>;