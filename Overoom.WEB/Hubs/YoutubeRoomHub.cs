using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Overoom.Application.Abstractions.Room.Exceptions;
using Overoom.Application.Abstractions.Room.Interfaces;
using Overoom.Domain.Room.BaseRoom.Exceptions;
using Overoom.Domain.Room.YoutubeRoom.Exceptions;
using Overoom.WEB.Hubs.Models;

namespace Overoom.WEB.Hubs;

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

    public async Task ChangeVideo(string id)
    {
        var data = GetData();
        await _roomManager.ChangeVideoAsync(data.RoomId, data.Id, id);
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", data.Id, id);
    }

    public async Task AddVideo(string url)
    {
        try
        {
            var data = GetData();
            var id = await _roomManager.AddVideoAsync(data.RoomId, data.Id, url);
            //TODO: Exceptions
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

    public override async Task OnConnectedAsync()
    {
        try
        {
            var data = GetData();
            var viewer = await _roomManager.ConnectAsync(data.RoomId, data.Id);
            await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
            var json = JsonSerializer.Serialize(new YoutubeViewerModel(viewer.Id, viewer.Username, viewer.AvatarUrl, (int) viewer.Time.TotalSeconds,
                    viewer.CurrentVideoId), new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            await Clients.Group(data.RoomIdString).SendAsync("Connect", json);
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            var error = ex switch
            {
                RoomNotFoundException => "Комната не найдена.",
                ViewerNotFoundException => "Зритель не найден.",
                _ => "Неизвестная ошибка."
            };
            await Clients.Caller.SendAsync("ReceiveMessage", error);
        }
    }
}