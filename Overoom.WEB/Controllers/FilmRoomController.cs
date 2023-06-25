using System.Security.Claims;
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

    [HttpGet]
    public async Task<IActionResult> CreateRoom(Guid filmId)
    {
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "CreateDefault" : "CreateUser", new { filmId });
    }

    [HttpGet]
    [Authorize(Policy = "Identity.Application")]
    public async Task<ActionResult> CreateUser(Guid filmId)
    {
        var roomData = await _roomService.CreateForUserAsync(filmId, User.Identity!.Name!);
        await RoomAuthentication.RoomAuthentication.AuthenticateViewerAsync(HttpContext, roomData.viewer,
            roomData.roomId, RoomType.Film);
        return RedirectToAction("Room", new { roomData.roomId });
    }


    [HttpGet]
    public ActionResult CreateDefault(Guid filmId) => View(new CreateFilmRoomParameters { FilmId = filmId });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDefault(CreateFilmRoomParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        var roomData = await _roomService.CreateAsync(model.FilmId, model.Cdn, model.Name);

        await HttpContext.SetAuthenticationDataAsync(roomData.viewer.Username, roomData.viewer.Id,
            roomData.viewer.AvatarUrl, roomData.roomId, RoomType.Film);
        return RedirectToAction("Room", new { roomData.roomId });
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal.FindFirstValue("RoomId") == roomId.ToString())
            return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "ConnectDefault" : "ConnectUser", new { roomId });
    }

    [HttpGet]
    public ActionResult ConnectDefault(Guid roomId) => View(new ConnectRoomParameters { RoomId = roomId });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConnectDefault(ConnectRoomParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        ViewerDto viewer = await _roomService.ConnectAsync(model.RoomId, model.Name);
        await RoomAuthentication.RoomAuthentication.AuthenticateViewerAsync(HttpContext, viewer, model.RoomId,
            RoomType.Film);
        return RedirectToAction("Room");
    }

    [HttpGet]
    public async Task<ActionResult> ConnectUser(Guid roomId)
    {
        ViewerDto viewer = await _roomService.ConnectForUserAsync(roomId, User.Identity!.Name!);
        await RoomAuthentication.RoomAuthentication.AuthenticateViewerAsync(HttpContext, viewer, roomId, RoomType.Film);
        return RedirectToAction("Room");
    }

    [Authorize(Policy = "FilmRoom")]
    public async Task<IActionResult> Room()
    {
        var id = User.GetRoomId();
        var viewerId = User.GetId();
        var roomDto = await _roomService.GetAsync(id);
        var model = _mapper.Map(roomDto, viewerId,
            Url.Action("Connect", "FilmRoom", new { roomId = id }, HttpContext.Request.Scheme)!);
        return View(model);
    }


    // private static FilmRoomViewModel Map(FilmRoomDto dto, Guid id, string url)
    // {
    //     var messages = dto.Messages.Select(Map);
    //     var viewers = dto.Viewers.Select(Map);
    //     var film = new FilmViewModel(dto.Film.Name, dto.Film.Url, dto.Film.Type);
    //     return new FilmRoomViewModel(messages, viewers, film, url, dto.OwnerId, id);
    // }
}