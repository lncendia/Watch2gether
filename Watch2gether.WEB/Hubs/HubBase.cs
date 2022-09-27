using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

public abstract class HubBase : Hub
{
    private readonly IRoomManager _roomManager;
    protected HubBase(IRoomManager roomManager) => _roomManager = roomManager;

    public async Task Send(string message)
    {
        var data = GetData();
        try
        {
            await _roomManager.SendMessageAsync(data.RoomId, data.Id, message);
        }
        catch
        {
            return;
        }

        await Clients.Group(data.RoomIdString).SendAsync("Send", data.Username, data.Id, data.AvatarFileName, message);
    }

    public async Task Pause(int seconds)
    {
        seconds++;
        var data = GetData();
        try
        {
            await _roomManager.SetPauseAsync(data.RoomId, data.Id, true, TimeSpan.FromSeconds(seconds));
        }
        catch
        {
            return;
        }

        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Pause", seconds, data.Id);
    }

    public async Task Play(int seconds)
    {
        seconds++;
        var data = GetData();
        try
        {
            await _roomManager.SetPauseAsync(data.RoomId, data.Id, false, TimeSpan.FromSeconds(seconds));
        }
        catch
        {
            return;
        }

        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Play", seconds, data.Id);
    }
    
    public override async Task OnConnectedAsync()
    {
        var data = GetData();
        try
        {
            await _roomManager.SetOnlineAsync(data.RoomId, data.Id, true);
        }
        catch
        {
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Connect", data.Username, data.Id);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var data = GetData();
        try
        {
            await _roomManager.SetOnlineAsync(data.RoomId, data.Id, false);
        }
        catch
        {
            return;
        }

        //await _roomDeleterManager.DeleteRoomIfEmpty(Guid.Parse(roomId));
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Leave", data.Id);
        await base.OnDisconnectedAsync(exception);
    }

    protected DataModel GetData()
    {
        var id = Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var username = Context.User.FindFirstValue(ClaimTypes.Name)!;
        var avatar = Context.User.FindFirstValue(ClaimTypes.Thumbprint)!;
        var roomId = Context.User.FindFirstValue("RoomId")!;
        return new DataModel(Guid.Parse(id), roomId, username, avatar);
    }
}