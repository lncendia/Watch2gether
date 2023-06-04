using Overoom.Application.Abstractions.Room.DTOs;

namespace Overoom.Application.Abstractions.Room.Interfaces;

public interface IRoomManager<TR, TV> where TR : RoomDto where TV : ViewerDto
{
    Task<TR> GetAsync(Guid roomId);
    Task SendMessageAsync(Guid roomId, int viewerId, string message);
    Task SetPauseAsync(Guid roomId, int viewerId, bool pause, TimeSpan time);
    Task DisconnectAsync(Guid roomId, int viewerId);
    Task<TV> ConnectAsync(Guid roomId, int viewerId);
    Task<TV> ConnectAsync(Guid roomId, string name);
    Task<TV> ConnectForUserAsync(Guid roomId, string email);
}