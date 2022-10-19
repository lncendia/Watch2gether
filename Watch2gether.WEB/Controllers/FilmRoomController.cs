using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.DTO.Rooms.Film;
using Watch2gether.Application.Abstractions.Exceptions.Films;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.Domain.Users.Exceptions;
using Watch2gether.WEB.Models.Room;
using Watch2gether.WEB.Models.Room.FilmRoom;
using Watch2gether.WEB.RoomAuthentication;

namespace Watch2gether.WEB.Controllers;

public class FilmRoomController : Controller
{
    private readonly IFilmRoomManager _roomService;

    public FilmRoomController(IFilmRoomManager roomService) => _roomService = roomService;

    [HttpGet]
    public async Task<IActionResult> CreateRoom(Guid filmId)
    {
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new CreateFilmRoomViewModel {FilmId = filmId});

        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateForUserAsync(filmId, data.Principal!.Identity!.Name!);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                FilmNotFoundException => "Фильм не найден",
                InvalidNicknameException => "Неверный формат имени",
                _ => "Произошла ошибка при создании комнаты"
            };
            ModelState.AddModelError("", text);
            return View(new CreateFilmRoomViewModel {FilmId = filmId});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Film);
        return RedirectToAction("Room", new {roomData.roomId});
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(CreateFilmRoomViewModel model)
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
                InvalidNicknameException => "Неверный формат имени",
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
        if (data.None) return View(new ConnectToRoomViewModel {RoomId = roomId});
        ViewerDto viewer;
        try
        {
            viewer = await _roomService.ConnectForUserAsync(roomId, data.Principal!.Identity!.Name!);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                RoomNotFoundException => "Комната не найдена",
                InvalidNicknameException => "Неверный формат имени",
                RoomIsFullException => "Комната заполнена",
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(new ConnectToRoomViewModel {RoomId = roomId});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, roomId, RoomType.Film);
        return RedirectToAction("Room");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(ConnectToRoomViewModel model)
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
                InvalidNicknameException => "Неверный формат имени",
                RoomIsFullException => "Комната заполнена",
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, model.RoomId, RoomType.Film);
        return RedirectToAction("Room");
    }

    [Authorize(Policy = "FilmRoom")]
    public async Task<IActionResult> Room()
    {
        var id = Guid.Parse(User.FindFirstValue("RoomId"));
        var viewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var roomDto = await _roomService.GetAsync(id, viewerId);
            return View(Map(roomDto, viewerId,
                Url.Action("Connect", "FilmRoom", new {roomId = id}, HttpContext.Request.Scheme)!));
        }
        catch (RoomNotFoundException)
        {
            return RedirectToAction("Index", "Home", new {message = "Комната не найдена"});
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