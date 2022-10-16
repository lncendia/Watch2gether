using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;

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

    public async Task ChangeSeries(int season, int number)
    {
        var data = GetData();
        //TODO: ChangeSeries
        await Clients.OthersInGroup(data.RoomIdString).SendAsync("Change", data.Id, season, number);
    }
}