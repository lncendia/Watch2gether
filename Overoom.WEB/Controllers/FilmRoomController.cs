using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.WEB.Contracts.Rooms;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.WEB.Controllers;

public class FilmRoomController : Controller
{
    private readonly IFilmRoomManager _roomService;
    private readonly Mappers.Abstractions.IFilmRoomMapper _mapper;

    public FilmRoomController(IFilmRoomManager roomService, Mappers.Abstractions.IFilmRoomMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
    }


    [HttpGet]
    public IActionResult Create(FilmParameters parameters)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home", new { message = "Не удалось создать комнату" });
        return View(parameters);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "User")]
    public async Task<ActionResult> CreateUser(CreateFilmRoomParameters model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home", new { message = "Не удалось создать комнату" });
        var roomData = await _roomService.CreateAsync(
            new CreateFilmRoomDto(model.IsOpen!.Value, model.FilmId, model.Cdn), User.GetId());
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomData.roomId,
            RoomType.Film);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAnonymously(CreateFilmRoomAnonymouslyParameters model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home", new { message = "Не удалось создать комнату" });
        var roomData = await _roomService.CreateAnonymouslyAsync(
            new CreateFilmRoomDto(model.IsOpen!.Value, model.FilmId, model.Cdn), model.Name!);
        var viewer = await _roomService.GetAsync(roomData.roomId, roomData.viewerId);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomData.roomId,
            RoomType.Film);
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
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomId,
            RoomType.Film);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(ConnectRoomAnonymouslyParameters model)
    {
        if (!ModelState.IsValid) View(model);
        var id = await _roomService.ConnectAnonymouslyAsync(model.RoomId, model.Name!);
        var viewer = await _roomService.GetAsync(model.RoomId, id);
        await HttpContext.SignInRoomAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, model.RoomId,
            RoomType.Film);
        return RedirectToAction("Room");
    }


    [Authorize(Policy = "FilmRoom")]
    public async Task<IActionResult> Room()
    {
        var id = User.GetRoomId();
        var viewerId = User.GetViewerId();
        var roomDto = await _roomService.GetAsync(id);
        var model = _mapper.Map(roomDto, viewerId,
            Url.Action("Connect", "FilmRoom", new { roomId = id }, HttpContext.Request.Scheme)!);
        return PartialView(model);
    }

    [Authorize(Policy = "FilmRoom")]
    public async Task<IActionResult> Leave()
    {
        await HttpContext.SignOutRoomAsync();
        return RedirectToAction("Index", "Home", new { message = "Вас выгнали из комнаты" });
    }
}