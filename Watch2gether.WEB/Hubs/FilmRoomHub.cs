using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

[Authorize(Policy = "FilmRoom")]
public class FilmRoomHub : HubBase
{
    private readonly IFilmRoomManager _roomManager;
    private readonly IRoomDeleterManager _roomDeleterManager;

    public FilmRoomHub(IFilmRoomManager roomService, IRoomDeleterManager roomDeleterManager) : base(roomService)
    {
        _roomManager = roomService;
        _roomDeleterManager = roomDeleterManager;
    }

    public async Task ChangeSeries(int season, int series)
    {
        var data = GetData();
        await _roomManager.ChangeSeries(data.RoomId, data.Id, season, series);
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", data.Id, season, series);
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var data = GetData();
            var viewer = await _roomManager.ConnectAsync(data.RoomId, data.Id);
            await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Connect",
                new FilmViewerModel(viewer.Id, viewer.Username, viewer.AvatarUrl, (int) viewer.Time.TotalSeconds, viewer.Season,
                    viewer.Series));
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
}