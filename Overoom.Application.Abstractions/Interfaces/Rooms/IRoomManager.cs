using Overoom.Application.Abstractions.DTO.Rooms;

namespace Overoom.Application.Abstractions.Interfaces.Rooms;

public interface IRoomManager
{
    Task SendMessageAsync(Guid roomId, Guid viewerId, string message);
    Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time);
    Task DisconnectAsync(Guid roomId, Guid viewerId);
}