using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

[Authorize(Policy = "YoutubeRoom")]
public class YoutubeRoomHub : HubBase
{
    private readonly IYoutubeRoomManager _roomManager;
    private readonly IRoomDeleterManager _roomDeleterManager;

    public YoutubeRoomHub(IYoutubeRoomManager roomService, IRoomDeleterManager roomDeleterManager) : base(roomService)
    {
        _roomManager = roomService;
        _roomDeleterManager = roomDeleterManager;
    }
    public async Task ChangeVideo(int index)
    {
        var data = GetData();
        //TODO: ChangeSeries
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", index);
    }

    public async Task AddVideo(string url)
    {
        try
        {
            var data = GetData();
            var id = await _roomManager.AddVideoAsync(data.RoomId, url);
            await Clients.Group(data.RoomIdString).SendAsync("AddVideo", id);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                InvalidVideoUrlException => "Ошибка. Неверный URL.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }

    public async Task RemoveVideo(string id)
    {
        try
        {
            var data = GetData();
            await _roomManager.RemoveVideoAsync(data.RoomId, id);
            await Clients.Group(data.RoomIdString).SendAsync("RemoveVideo", id);
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Ошибка. Комната не найдена.",
                ViewerNotFoundException => "Ошибка. Зритель не найден.",
                InvalidVideoUrlException => "Ошибка. Неверный URL.",
                _ => "Неизвестная ошибка"
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }
}