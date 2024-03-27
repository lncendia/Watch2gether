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
    public YoutubeRoom(User user, IReadOnlyCollection<Server> servers, bool isOpen) : base(user, servers, isOpen)
    {
        AddDomainEvent(new YoutubeRoomCreatedDomainEvent
        {
            Owner = user,
            Room = this,
            Server = servers.First(s => s.Id == ServerId)
        });
    }
    
    public override void Connect(User user, string? code)
    {
        base.Connect(user, code);
        AddDomainEvent(new YoutubeRoomUserConnectedDomainEvent(this, user));
    }

    public required bool VideoAccess { get; init; }
}