using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Films.Domain.Rooms.YoutubeRoom.Exceptions;
using Overoom.Infrastructure.Web.Hubs.Models;

namespace Overoom.Infrastructure.Web.Hubs;

[Authorize(Policy = "YoutubeRoom")]
public class YoutubeRoomHub : HubBase
{
    private readonly IYoutubeRoomManager _roomManager;

    public YoutubeRoomHub(IYoutubeRoomManager roomService) : base(roomService)
    {
        _roomManager = roomService;
    }

    public async Task ChangeVideo(string id)
    {
        var data = GetData();
        try
        {
            await _roomManager.ChangeVideoAsync(data.RoomId, data.Id, id);
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", data.Id, id);
        }
        catch (Exception ex)
        {
            await HandleException(ex, data);
        }
    }

    public async Task AddVideo(string url)
    {
        var data = GetData();
        try
        {
            var id = await _roomManager.AddVideoAsync(data.RoomId, data.Id, new Uri(url));
            await Clients.OthersInGroup(data.RoomIdString).SendAsync("AddVideo", id);
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
            var viewerViewModel = new YoutubeViewerModel(viewer.Id, viewer.Username, viewer.AvatarUrl,
                (int)viewer.Time.TotalSeconds, viewer.CurrentVideoId, viewer.Allows.Beep, viewer.Allows.Scream,
                viewer.Allows.Change);
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
            case InvalidVideoUrlException:
                error = "Неверный URL.";
                break;
            case UriFormatException:
                error = "Неверный формат URL.";
                break;
            default:
                return base.HandleException(ex, data);
        }

        return Clients.Caller.SendAsync("Error", data.Id, error);
    }
}