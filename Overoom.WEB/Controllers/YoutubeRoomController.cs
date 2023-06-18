using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.Application.Abstractions.Rooms.Exceptions;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.YoutubeRoom.Exceptions;
using Overoom.WEB.Models.Room;
using Overoom.WEB.Models.Room.YoutubeRoom;
using Overoom.WEB.RoomAuthentication;

namespace Overoom.WEB.Controllers;

public class YoutubeRoomController : Controller
{
    private readonly IYoutubeRoomManager _roomService;

    public YoutubeRoomController(IYoutubeRoomManager roomService) => _roomService = roomService;

    [HttpGet]
    public async Task<IActionResult> CreateRoom()
    {
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(data.None ? "CreateDefault" : "CreateUser");
    }

    [HttpGet]
    public ActionResult CreateDefault() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDefault(CreateYoutubeRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateAsync(model.Url, model.Name, model.AddAccess);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UriFormatException => "Неверный формат ссылки",
                ViewerInvalidNicknameException => "Неверный формат имени",
                InvalidVideoUrlException => "Неверный формат ссылки на видео",
                _ => "Произошла ошибка при создании комнаты"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Youtube);
        return RedirectToAction("Room", new {roomData.roomId});
    }

    [HttpGet]
    [Authorize(Policy = "Identity.Application")]
    public ActionResult CreateUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Identity.Application")]
    public async Task<IActionResult> CreateUser(CreateYoutubeRoomForUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateForUserAsync(model.Url, TODO, model.AddAccess);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                UriFormatException => "Неверный формат ссылки",
                ViewerInvalidNicknameException => "Неверный формат имени",
                InvalidVideoUrlException => "Неверный формат ссылки на видео",
                _ => "Произошла ошибка при создании комнаты"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Youtube);
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
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, model.RoomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [HttpGet]
    [Authorize(Policy = "Identity.Application")]
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
                _ => "Произошла ошибка при подключении"
            };
            return RedirectToAction("Index", "Home", new {message = text});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, roomId, RoomType.Youtube);
        return RedirectToAction("Room");
    }


    [Authorize(Policy = "YoutubeRoom")]
    public async Task<IActionResult> Room()
    {
        var id = Guid.Parse(User.FindFirstValue("RoomId"));
        var viewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var roomDto = await _roomService.GetAsync(id);
            return View(Map(roomDto, viewerId,
                Url.Action("Connect", "YoutubeRoom", new {roomId = id}, HttpContext.Request.Scheme)!));
        }
        catch (RoomNotFoundException)
        {
            return RedirectToAction("Index", "Home", new {message = "Комната не найдена"});
        }
    }


    private static YoutubeRoomViewModel Map(YoutubeRoomDto dto, Guid id, string url)
    {
        var messages = dto.Messages.Select(Map);
        var viewers = dto.Viewers.Select(Map);
        return new YoutubeRoomViewModel(messages, viewers, url, dto.OwnerId, id, dto.Ids, dto.AddAccess);
    }

    private static YoutubeViewerViewModel Map(YoutubeViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time, dto.CurrentVideoId);

    private static YoutubeMessageViewModel Map(YoutubeMessageDto dto)
    {
        var viewer = Map(dto.Viewer);
        return new YoutubeMessageViewModel(dto.Text, dto.CreatedAt, viewer);
    }
}