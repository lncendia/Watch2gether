using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Films.Application.Abstractions;
using Films.Application.Abstractions.Rooms.DTOs.Youtube;
using Films.Application.Abstractions.Rooms.Interfaces;
using Films.Infrastructure.Web.Contracts.Rooms;
using Films.Infrastructure.Web.RoomAuthentication;
using Films.Infrastructure.Web.Authentication;
using Abstractions_IYoutubeRoomMapper = Films.Infrastructure.Web.Mappers.Abstractions.IYoutubeRoomMapper;
using IYoutubeRoomMapper = Films.Infrastructure.Web.Mappers.Abstractions.IYoutubeRoomMapper;
using Mappers_Abstractions_IYoutubeRoomMapper = Films.Infrastructure.Web.Mappers.Abstractions.IYoutubeRoomMapper;

namespace Films.Infrastructure.Web.Controllers;

public class RoomController : Controller
{
    private readonly IYoutubeRoomManager _roomService;
    private readonly Mappers_Abstractions_IYoutubeRoomMapper _mapper;

    public RoomController(IYoutubeRoomManager roomService, Mappers_Abstractions_IYoutubeRoomMapper mapper)
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
            new CreateYoutubeRoomDto(model.IsOpen!.Value, new Uri(model.Url!), model.Access), User.GetId());
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Id, roomData.roomId,
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
            new CreateYoutubeRoomDto(model.IsOpen!.Value, new Uri(model.Url!), model.Access), model.Name!);
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Id, roomData.roomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal!.GetRoomId() == roomId) return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new ConnectRoomAnonymouslyParameters { RoomId = roomId });

        var id = await _roomService.ConnectAsync(roomId, User.GetId());
        var viewer = await _roomService.GetAsync(roomId, id);
        await HttpContext.SignInRoomAsync(viewer.Id, roomId, RoomType.Youtube);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(ConnectRoomAnonymouslyParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        var id = await _roomService.ConnectAnonymouslyAsync(model.RoomId, model.Name!);
        var viewer = await _roomService.GetAsync(model.RoomId, id);
        await HttpContext.SignInRoomAsync(viewer.Id, model.RoomId, RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [Authorize(Policy = "YoutubeRoom")]
    public async Task<IActionResult> Room()
    {
        var id = User.GetRoomId();
        var viewerId = User.GetViewerId();
        var roomDto = await _roomService.GetAsync(id);
        var roomViewModel = _mapper.Map(roomDto, viewerId,
            Url.Action("Connect", "Room", new { roomId = id }, HttpContext.Request.Scheme)!);
        return PartialView(roomViewModel);
    }
}