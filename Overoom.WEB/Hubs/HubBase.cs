using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.Exceptions;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.WEB.Hubs.Models;

namespace Overoom.WEB.Hubs;

public abstract class HubBase : Hub
{
    private readonly IRoomManager<RoomDto, ViewerDto> _roomManager;

    protected HubBase(IRoomManager<RoomDto, ViewerDto> roomManager) => _roomManager = roomManager;

    public async Task Send(string message)
    {
        try
        {
            var data = GetData();
            await _roomManager.SendMessageAsync(data.RoomId, data.Id, message);
            await Clients.Group(data.RoomIdString).SendAsync("Send", data.Id, message);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Комната не найдена.",
                ViewerNotFoundException => "Зритель не найден.",
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
                RoomNotFoundException => "Комната не найдена.",
                ViewerNotFoundException => "Зритель не найден.",
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
                RoomNotFoundException => "Комната не найдена.",
                ViewerNotFoundException => "Зритель не найден.",
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
            await _roomManager.DisconnectAsync(data.RoomId, data.Id);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Leave", data.Id);
            await base.OnDisconnectedAsync(exception);
        }
        catch
        {
            // ignored
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