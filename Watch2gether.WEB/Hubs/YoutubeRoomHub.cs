using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.WEB.Hubs.Models;

namespace Watch2gether.WEB.Hubs;

[Authorize(Policy = ApplicationConstants.RoomScheme)]
public class YoutubeRoomHub : HubBase
{
    private readonly IYoutubeRoomManager _roomManager;
    private readonly IRoomDeleterManager _roomDeleterManager;

    public YoutubeRoomHub(IYoutubeRoomManager roomService, IRoomDeleterManager roomDeleterManager) : base(roomService)
    {
        _roomManager = roomService;
        _roomDeleterManager = roomDeleterManager;
    }
}