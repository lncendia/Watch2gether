using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
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


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "User")]
    public async Task<ActionResult> CreateUser(CreateYoutubeRoomParameters model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home", new { message = "Не удалось создать комнату" });
        var roomData = await _roomService.CreateAsync(
            new CreateYoutubeRoomDto(model.IsOpen!.Value, new Uri(model.Url!), model.AddAccess), User.GetId());
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomData.roomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAnonymously(CreateYoutubeRoomAnonymouslyParameters model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home", new { message = "Не удалось создать комнату" });
        var roomData = await _roomService.CreateAnonymouslyAsync(
            new CreateYoutubeRoomDto(model.IsOpen!.Value, new Uri(model.Url!), model.AddAccess), model.Name!);
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomData.roomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal!.GetId() == roomId) return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new ConnectRoomAnonymouslyParameters { RoomId = roomId });

        var id = await _roomService.ConnectAsync(roomId, User.GetId());
        var viewer = await _roomService.GetAsync(roomId, id);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomId, RoomType.Youtube);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(ConnectRoomAnonymouslyParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        var id = await _roomService.ConnectAnonymouslyAsync(model.RoomId, model.Name!);
        var viewer = await _roomService.GetAsync(model.RoomId, id);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, model.RoomId, RoomType.Youtube);
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
        return PartialView(roomViewModel);
    }
}