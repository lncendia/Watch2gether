using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Rooms.Exceptions;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

[Authorize(Policy = "RoomTemporary")]
public class RoomHub : Hub
{
    private readonly IRoomManager _roomManager;
    private readonly IRoomDeleterManager _roomDeleterManager;

    public RoomHub(IRoomManager roomService, IRoomDeleterManager roomDeleterManager)
    {
        _roomManager = roomService;
        _roomDeleterManager = roomDeleterManager;
    }

    public async Task Send(string message)
    {
        var data = GetData();
        try
        {
            await _roomManager.SendMessageAsync(data.RoomId, data.Id, message);
        }
        catch (MessageLengthException)
        {
            return;
        }

        await Clients.OthersInGroup(data.RoomIdString)
            .SendAsync("Send", data.Username, data.Id, data.AvatarFileName, message);
    }

    public async Task Pause(int seconds)
    {
        seconds++;
        var data = GetData();
        await _roomManager.SetPauseAsync(data.RoomId, data.Id, true, TimeSpan.FromSeconds(seconds));
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Pause", seconds, data.Id);
    }

    public async Task Play(int seconds)
    {
        seconds++;
        var data = GetData();
        await _roomManager.SetPauseAsync(data.RoomId, data.Id, false, TimeSpan.FromSeconds(seconds));
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Play", seconds, data.Id);
    }

    public async Task ChangeSeries(int season, int number)
    {
        var data = GetData();
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", data.Id, season, number);
    }

    public override async Task OnConnectedAsync()
    {
        var data = GetData();
        await _roomManager.SetOnlineAsync(data.RoomId, data.Id, true);
        await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Connect", data.Username, data.Id);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var data = GetData();
        await _roomManager.SetOnlineAsync(data.RoomId, data.Id, false);
        //await _roomDeleterManager.DeleteRoomIfEmpty(Guid.Parse(roomId));
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Leave", data.Username, data.Id);
        await base.OnDisconnectedAsync(exception);
    }

    private DataModel GetData()
    {
        var id = Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var username = Context.User.FindFirstValue(ClaimTypes.Name)!;
        var avatar = Context.User.FindFirstValue(ClaimTypes.Thumbprint)!;
        var roomId = Context.User.FindFirstValue("RoomId")!;
        return new DataModel(Guid.Parse(id), roomId, username, avatar);
    }
}