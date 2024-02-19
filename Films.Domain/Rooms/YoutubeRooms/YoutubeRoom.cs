using System.Collections.Generic;
using Films.Domain.Rooms.BaseRoom;
using Films.Domain.Rooms.YoutubeRooms.Events;
using Films.Domain.Servers;
using Films.Domain.Users;

namespace Films.Domain.Rooms.YoutubeRooms;

/// <summary> 
/// Класс, представляющий комнату ютуб. 
/// </summary> 
public class YoutubeRoom : Room
{
    /// <summary> 
    /// Класс, представляющий комнату ютуб. 
    /// </summary> 
    public YoutubeRoom(User user, IEnumerable<Server> servers, bool isOpen) : base(user, servers, isOpen)
    {
        AddDomainEvent(new YoutubeRoomCreatedDomainEvent
        {
            Owner = user,
            Room = this
        });
    }

    public required bool VideoAccess { get; init; }
}