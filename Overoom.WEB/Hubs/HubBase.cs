using Microsoft.AspNetCore.SignalR;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.WEB.Hubs.Models;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.WEB.Hubs;

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
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Message", data.Id, message);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task Seek(int seconds)
    {
        seconds++;
        var data = GetData();
        try
        {
            await _roomManager.SeekAsync(data.RoomId, data.Id, TimeSpan.FromSeconds(seconds));
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Seek", data.Id, seconds);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task Pause(bool pause, int seconds)
    {
        seconds++;
        var data = GetData();
        try
        {
            await _roomManager.PauseAsync(data.RoomId, data.Id, pause);
            await _roomManager.SeekAsync(data.RoomId, data.Id, TimeSpan.FromSeconds(seconds));
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Pause", data.Id, pause, seconds);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task FullScreen(bool fullScreen)
    {
        var data = GetData();
        try
        {
            await _roomManager.FullScreenAsync(data.RoomId, data.Id, fullScreen);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("FullScreen", data.Id, fullScreen);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task Beep(int target)
    {
        var data = GetData();
        try
        {
            await _roomManager.BeepAsync(data.RoomId, data.Id, target);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Beep", data.Id, target);
        }
        catch (ActionNotAllowedException)
        {
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task Scream(int target)
    {
        var data = GetData();
        try
        {
            await _roomManager.ScreamAsync(data.RoomId, data.Id, target);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Scream", data.Id, target);
        }
        catch (ActionNotAllowedException)
        {
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }


    public async Task Kick(int target)
    {
        var data = GetData();
        try
        {
            await _roomManager.KickAsync(data.RoomId, data.Id, target);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Kick", data.Id, target);
        }
        catch (ActionNotAllowedException)
        {
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }


    public async Task ChangeName(int target, string name)
    {
        var data = GetData();
        try
        {
            await _roomManager.ChangeNameAsync(data.RoomId, data.Id, target, name);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("ChangeName", data.Id, target, name);
        }
        catch (ActionNotAllowedException)
        {
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task Type()
    {
        var data = GetData();
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Type", data.Id);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var data = GetData();
        try
        {
            await _roomManager.DisconnectAsync(data.RoomId, data.Id);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Leave", data.Id);
            await base.OnDisconnectedAsync(exception);
        }
        catch
        {
            // ignored
        }
    }

    protected DataModel GetData()
    {
        var id = Context.User!.GetViewerId();
        var username = Context.User!.GetName();
        var avatar = Context.User!.GetAvatar();
        var roomId = Context.User!.GetRoomId();
        return new DataModel(id, roomId, username, avatar);
    }

    protected Task HandleException(Exception ex, DataModel data)
    {
        var error = ex switch
        {
            RoomNotFoundException => "Комната не найдена.",
            ViewerNotFoundException => "Зритель не найден.",
            _ => "Неизвестная ошибка"
        };
        return Clients.Caller.SendAsync("ReceiveMessage", data.Id, error);
    }
}