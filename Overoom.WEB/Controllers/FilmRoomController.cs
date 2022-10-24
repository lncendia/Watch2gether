using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.DTO.Rooms;
using Overoom.Application.Abstractions.DTO.Rooms.Film;
using Overoom.Application.Abstractions.Exceptions.Films;
using Overoom.Application.Abstractions.Exceptions.Rooms;
using Overoom.Application.Abstractions.Exceptions.Users;
using Overoom.Application.Abstractions.Interfaces.Rooms;
using Overoom.WEB.Models.Room;
using Overoom.WEB.Models.Room.FilmRoom;
using Overoom.WEB.RoomAuthentication;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Users.Exceptions;

namespace Overoom.WEB.Controllers;

public class FilmRoomController : Controller
{
    private readonly IFilmRoomManager _roomService;

    public FilmRoomController(IFilmRoomManager roomService) => _roomService = roomService;

    [HttpGet]
    public async Task<IActionResult> CreateRoom(Guid filmId)
    {
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "CreateDefault" : "CreateUser", new {filmId});
    }

    [HttpGet]
    [Authorize(Policy = "Identity.Application")]
    public async Task<ActionResult> CreateUser(Guid filmId)
    {
        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateForUserAsync(filmId, User.Identity!.Name!);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                FilmNotFoundException => "Фильм не найден",
                ViewerInvalidNicknameException => "Неверный формат имени",
                _ => "Произошла ошибка при создании комнаты"
            };
            return RedirectToAction("Index", "Home", new {message = text});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Film);
        return RedirectToAction("Room", new {roomData.roomId});
    }


    [HttpGet]
    public ActionResult CreateDefault(Guid filmId) => View(new CreateFilmRoomViewModel {FilmId = filmId});

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDefault(CreateFilmRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateAsync(model.FilmId, model.Name);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                FilmNotFoundException => "Фильм не найден",
                ViewerInvalidNicknameException => "Неверный формат имени",
                _ => "Произошла ошибка при создании комнаты"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Film);
        return RedirectToAction("Room", new {roomData.roomId});
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal.FindFirstValue("RoomId") == roomId.ToString())
            return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "ConnectDefault" : "ConnectUser", new {roomId});
    }

    [HttpGet]
    public ActionResult ConnectDefault(Guid roomId) => View(new ConnectToRoomViewModel {RoomId = roomId});

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConnectDefault(ConnectToRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        ViewerDto viewer;
        try
        {
            viewer = await _roomService.ConnectAsync(model.RoomId, model.Name);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                RoomNotFoundException => "Комната не найдена",
                ViewerInvalidNicknameException => "Неверный формат имени",
                RoomIsFullException => "Комната заполнена",
                FilmNotFoundException => "Фильм не найден",
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, model.RoomId, RoomType.Film);
        return RedirectToAction("Room");
    }

    [HttpGet]
    public async Task<ActionResult> ConnectUser(Guid roomId)
    {
        ViewerDto viewer;
        try
        {
            viewer = await _roomService.ConnectForUserAsync(roomId, User.Identity!.Name!);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                RoomNotFoundException => "Комната не найдена",
                ViewerInvalidNicknameException => "Неверный формат имени",
                RoomIsFullException => "Комната заполнена",
                FilmNotFoundException => "Фильм не найден",
                _ => "Произошла ошибка при подключении"
            };
            return RedirectToAction("Index", "Home", new {message = text});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, roomId, RoomType.Film);
        return RedirectToAction("Room");
    }

    [Authorize(Policy = "FilmRoom")]
    public async Task<IActionResult> Room()
    {
        var id = Guid.Parse(User.FindFirstValue("RoomId"));
        var viewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var roomDto = await _roomService.GetAsync(id);
            return View(Map(roomDto, viewerId,
                Url.Action("Connect", "FilmRoom", new {roomId = id}, HttpContext.Request.Scheme)!));
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                FilmNotFoundException => "Фильм не найден",
                ViewerInvalidNicknameException => "Неверный формат имени",
                _ => "Произошла ошибка при создании комнаты"
            };
            return RedirectToAction("Index", "Home", new {message = text});
        }
    }


    private static FilmRoomViewModel Map(FilmRoomDto dto, Guid id, string url)
    {
        var messages = dto.Messages.Select(Map);
        var viewers = dto.Viewers.Select(Map);
        var film = new FilmViewModel(dto.Film.Name, dto.Film.Url, dto.Film.Type);
        return new FilmRoomViewModel(messages, viewers, film, url, dto.OwnerId, id);
    }

    private static FilmViewerViewModel Map(FilmViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time, dto.Season, dto.Series);

    private static FilmMessageViewModel Map(FilmMessageDto dto)
    {
        var viewer = Map(dto.Viewer);
        return new FilmMessageViewModel(dto.Text, dto.CreatedAt, viewer);
    }
}