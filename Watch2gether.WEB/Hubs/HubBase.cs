using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

public abstract class HubBase : Hub
{
    private readonly IRoomManager _roomManager;
    protected HubBase(IRoomManager roomManager) => _roomManager = roomManager;

    public async Task Send(string message)
    {
        try
        {
            var data = GetData();
            await _roomManager.SendMessageAsync(data.RoomId, data.Id, message);
            await Clients.Group(data.RoomIdString).SendAsync("Send", data.Username, data.Id, data.AvatarFileName, message);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }

    public async Task Pause(int seconds)
    {
        try
        {
            seconds++;
            var data = GetData();
            await _roomManager.SetPauseAsync(data.RoomId, data.Id, true, TimeSpan.FromSeconds(seconds));
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Pause", seconds, data.Id);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }

    public async Task Play(int seconds)
    {
        try
        {
            seconds++;
            var data = GetData();
            await _roomManager.SetPauseAsync(data.RoomId, data.Id, false, TimeSpan.FromSeconds(seconds));
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Play", seconds, data.Id);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var data = GetData();
            await _roomManager.SetOnlineAsync(data.RoomId, data.Id, true);
            await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Connect", data.Username, data.Id);
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var data = GetData();
            await _roomManager.SetOnlineAsync(data.RoomId, data.Id, false);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Leave", data.Id);
            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }

        //await _roomDeleterManager.DeleteRoomIfEmpty(Guid.Parse(roomId));
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