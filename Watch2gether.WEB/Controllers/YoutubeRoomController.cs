using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Users.Exceptions;
using Watch2gether.WEB.Models.Room;
using Watch2gether.WEB.Models.Room.YoutubeRoom;
using Watch2gether.WEB.RoomAuthentication;

namespace Watch2gether.WEB.Controllers;

public class YoutubeRoomController : Controller
{
    private readonly IYoutubeRoomManager _roomService;

    public YoutubeRoomController(IYoutubeRoomManager roomService) => _roomService = roomService;

    [HttpGet]
    public async Task<IActionResult> CreateRoom(string link)
    {
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new CreateYoutubeRoomViewModel {Url = link});

        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateForUserAsync(link, data.Principal!.Identity!.Name!);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UserNotFoundException => "Пользователь с таким email не найден",
                UriFormatException => "Неверный формат ссылки",
                InvalidNicknameException => "Неверный формат имени",
                _ => "Произошла ошибка при создании комнаты"
            };
            ModelState.AddModelError("", text);
            return View(new CreateYoutubeRoomViewModel {Url = link});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, roomData.viewer, roomData.roomId,
            RoomType.Youtube);
        return RedirectToAction("Room", new {roomData.roomId});
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(CreateYoutubeRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        (Guid roomId, ViewerDto viewer) roomData;
        try
        {
            roomData = await _roomService.CreateAsync(model.Url, model.Name);
        }
        catch (Exception ex)
        {
            var text = ex switch
            {
                UriFormatException => "Неверный формат ссылки",
                InvalidNicknameException => "Неверный формат имени",
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
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(new ConnectToRoomViewModel {RoomId = roomId});
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, roomId, RoomType.Youtube);
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
                _ => "Произошла ошибка при подключении"
            };
            ModelState.AddModelError("", text);
            return View(model);
        }

        await RoomAuthentication.RoomAuthentication.AuthenticateAsync(HttpContext, viewer, model.RoomId,
            RoomType.Youtube);
        return RedirectToAction("Room");
    }

    [Authorize(Policy = "YoutubeRoom")]
    public async Task<IActionResult> Room()
    {
        var id = Guid.Parse(User.FindFirstValue("RoomId"));
        var viewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var roomDto = await _roomService.GetAsync(id, viewerId);
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
        var messages = dto.Messages.Select(x => new MessageViewModel(x.Text, x.CreatedAt, Map(x.Viewer)))
            .ToList();
        var viewers = dto.Viewers.Select(Map).ToList();
        return new YoutubeRoomViewModel(messages, viewers, url, dto.OwnerId, viewers.First(x => x.Id == id), dto.Ids);
    }

    private static ViewerViewModel Map(ViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time);
}