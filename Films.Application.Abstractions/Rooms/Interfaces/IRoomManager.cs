namespace Films.Application.Abstractions.Rooms.Interfaces;

public interface IRoomManager
{
    Task SendMessageAsync(Guid roomId, int viewerId, string message);
    Task PauseAsync(Guid roomId, int viewerId, bool pause);
    Task FullScreenAsync(Guid roomId, int viewerId, bool pause);
    Task SeekAsync(Guid roomId, int viewerId, TimeSpan time);
    Task DisconnectAsync(Guid roomId, int viewerId);

    Task<int> ConnectAnonymouslyAsync(Guid roomId, string name);
    Task<int> ConnectAsync(Guid roomId, Guid userId);
    Task ReConnectAsync(Guid roomId, int viewerId);

    Task BeepAsync(Guid roomId, int viewerId, int target);
    Task KickAsync(Guid roomId, int viewerId, int target);
    Task ScreamAsync(Guid roomId, int viewerId, int target);
    Task ChangeNameAsync(Guid roomId, int viewerId, int target, string name);
}