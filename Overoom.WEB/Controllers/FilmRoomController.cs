using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs;
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

    [HttpPost]
    [Authorize(Policy = "User")]
    public async Task<ActionResult> CreateUser(CreateFilmRoomForUserParameters model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Film", "Film", new { id = model.FilmId });
        var roomData = await _roomService.CreateForUserAsync(model.FilmId, model.Cdn, User.GetId());
        await HttpContext.SetAuthenticationDataAsync(roomData.viewer.Username, roomData.viewer.Id,
            roomData.viewer.AvatarUrl, roomData.roomId, RoomType.Film);
        return RedirectToAction("Room", new { roomData.roomId });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDefault(CreateFilmRoomParameters model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Film", "Film", new { id = model.FilmId });
        var roomData = await _roomService.CreateAsync(model.FilmId, model.Cdn, model.Name);

        await HttpContext.SetAuthenticationDataAsync(roomData.viewer.Username, roomData.viewer.Id,
            roomData.viewer.AvatarUrl, roomData.roomId, RoomType.Film);
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
            RoomType.Film);
        return RedirectToAction("Room");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ConnectUser(Guid roomId)
    {
        ViewerDto viewer = await _roomService.ConnectForUserAsync(roomId, User.GetId());
        await HttpContext.SetAuthenticationDataAsync(viewer.Username, viewer.Id, viewer.AvatarUrl, roomId,
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
        return View(model);
    }
}