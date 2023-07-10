using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.WEB.Contracts.Rooms;
using Overoom.WEB.RoomAuthentication;
using IYoutubeRoomMapper = Overoom.WEB.Mappers.Abstractions.IYoutubeRoomMapper;

namespace Overoom.WEB.Controllers;

public class YoutubeRoomController : Controller
{
    private readonly IYoutubeRoomManager _roomService;
    private readonly IYoutubeRoomMapper _mapper;

    public YoutubeRoomController(IYoutubeRoomManager roomService, IYoutubeRoomMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDefault(CreateYoutubeRoomParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        var roomData = await _roomService.CreateAsync(model.Url, model.Name, model.AddAccess);

        await HttpContext.SetAuthenticationDataAsync(roomData.viewer.Username, roomData.viewer.Id,
            roomData.viewer.AvatarUrl, roomData.roomId, RoomType.Youtube);
        return RedirectToAction("Room", new { roomData.roomId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> CreateUser(CreateYoutubeRoomForUserParameters model)
    {
        if (!ModelState.IsValid) return View(model);

        var roomData = await _roomService.CreateForUserAsync(model.Url, User.GetId(), model.AddAccess);

        await HttpContext.SetAuthenticationDataAsync(roomData.viewer.Username, roomData.viewer.Id,
            roomData.viewer.AvatarUrl, roomData.roomId, RoomType.Youtube);
        return RedirectToAction("Room", new { roomData.roomId });
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal!.GetId() == roomId) return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "ConnectDefault" : "ConnectUser", new { roomId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConnectDefault(ConnectRoomParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        ViewerDto viewer = await _roomService.ConnectAsync(model.RoomId, model.Name);
        await HttpContext.SetAuthenticationDataAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, model.RoomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [HttpGet]
    [Authorize(Policy = "User")]
    public async Task<ActionResult> ConnectUser(Guid roomId)
    {
        ViewerDto viewer = await _roomService.ConnectForUserAsync(roomId, User.GetId());
        await HttpContext.SetAuthenticationDataAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [Authorize(Policy = "YoutubeRoom")]
    public async Task<IActionResult> Room()
    {
        var id = User.GetRoomId();
        var viewerId = User.GetViewerId();
        var roomDto = await _roomService.GetAsync(id);
        var roomViewModel = _mapper.Map(roomDto, viewerId,
            Url.Action("Connect", "YoutubeRoom", new { roomId = id }, HttpContext.Request.Scheme)!);
        return View(roomViewModel);
    }


    // private static YoutubeRoomViewModel Map(YoutubeRoomDto dto, Guid id, string url)
    // {
    //     var messages = dto.Messages.Select(Map);
    //     var viewers = dto.Viewers.Select(Map);
    //     return new YoutubeRoomViewModel(messages, viewers, url, dto.OwnerId, id, dto.Ids, dto.AddAccess);
    // }
    //
    // private static YoutubeViewerViewModel Map(YoutubeViewerDto dto) =>
    //     new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time, dto.CurrentVideoId);
    //
    // private static YoutubeMessageViewModel Map(YoutubeMessageDto dto)
    // {
    //     var viewer = Map(dto.Viewer);
    //     return new YoutubeMessageViewModel(dto.Text, dto.CreatedAt, viewer);
    // }
}