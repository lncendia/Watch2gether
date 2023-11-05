using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.WEB.Hubs.Models;

namespace Overoom.WEB.Hubs;

[Authorize(Policy = "FilmRoom")]
public class FilmRoomHub : HubBase
{
    private readonly IFilmRoomManager _roomManager;

    public FilmRoomHub(IFilmRoomManager roomService) : base(roomService)
    {
        _roomManager = roomService;
    }

    public async Task ChangeSeries(int season, int series)
    {
        var data = GetData();
        try
        {
            await _roomManager.ChangeSeries(data.RoomId, data.Id, season, series);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("ChangeSeries", data.Id, season, series);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var data = GetData();
        try
        {
            await _roomManager.ReConnectAsync(data.RoomId, data.Id);
            var viewer = await _roomManager.GetAsync(data.RoomId, data.Id);
            await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomIdString);
            var viewerViewModel = new FilmViewerModel(viewer.Id, viewer.Username, viewer.AvatarUrl,
                (int)viewer.Time.TotalSeconds, viewer.Season, viewer.Series, viewer.Allows.Beep,
                viewer.Allows.Scream, viewer.Allows.Change);
            await Clients.Group(data.RoomIdString).SendAsync("Connect", viewerViewModel);
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    protected override Task HandleException(Exception ex, DataModel data)
    {
        string? error;
        switch (ex)
        {
            default:
                return base.HandleException(ex, data);
        }
    }
}