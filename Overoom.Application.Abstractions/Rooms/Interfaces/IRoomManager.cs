namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IRoomManager
{
    Task SendMessageAsync(Guid roomId, int viewerId, string message);
    Task SetPauseAsync(Guid roomId, int viewerId, bool pause, TimeSpan time);
    Task DisconnectAsync(Guid roomId, int viewerId);
}