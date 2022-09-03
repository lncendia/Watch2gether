using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.WEB.Models.Room;

namespace Watch2gether.WEB.Controllers;

public class RoomController : Controller
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> CreateRoom(Guid filmId)
    {
        //TODO: Exceptions
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new CreateRoomViewModel {FilmId = filmId});
        
        var roomData = await _roomService.CreateForUserAsync(filmId, data.Principal!.Identity!.Name!);
        await AuthenticateUser(roomData.viewer, roomData.roomId);
        return RedirectToAction("Room", new {roomData.roomId});
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(CreateRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var data = await _roomService.CreateAsync(model.FilmId, model.Name);
        await AuthenticateUser(data.viewer, data.roomId);
        return RedirectToAction("Room", new {data.roomId});
    }


    [HttpGet]
    public async Task<IActionResult> Connect(Guid roomId)
    {
        var roomData = await HttpContext.AuthenticateAsync(ApplicationConstants.RoomScheme);
        if (!roomData.None && roomData.Principal.FindFirstValue("RoomId") == roomId.ToString())
            return RedirectToAction("Room");
        var data = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (data.None) return View(new ConnectToRoomViewModel {RoomId = roomId});
        
        var viewer = await _roomService.ConnectForUserAsync(roomId, data.Principal!.Identity!.Name!);
        await AuthenticateUser(viewer, roomId);
        return RedirectToAction("Room");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(ConnectToRoomViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var viewer = await _roomService.ConnectAsync(model.RoomId, model.Name);
        await AuthenticateUser(viewer, model.RoomId);
        return RedirectToAction("Room");
    }

    [Authorize(Policy = "RoomTemporary")]
    public async Task<IActionResult> Room()
    {
        var id = Guid.Parse(User.FindFirstValue("RoomId"));
        var viewerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var roomDto = await _roomService.GetAsync(id, viewerId);
        return View(Map(roomDto, viewerId,
            Url.Action("Connect", "Room", new {roomId = id}, HttpContext.Request.Scheme)!));
    }


    private static RoomViewModel Map(RoomDto dto, Guid id, string url)
    {
        var messages = dto.Messages.Select(x => new MessageViewModel(x.Text, x.CreatedAt.ToLocalTime(), Map(x.Viewer)))
            .ToList();
        var viewers = dto.Viewers.Select(Map).ToList();
        var film = new FilmViewModel(dto.Film.Name, dto.Film.Url);
        return new RoomViewModel(messages, viewers, film, url, dto.OwnerId, viewers.First(x => x.Id == id));
    }

    private static ViewerViewModel Map(ViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time);


    private async Task AuthenticateUser(ViewerDto viewer, Guid roomId) => await HttpContext.SignInAsync(
        ApplicationConstants.RoomScheme,
        new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, viewer.Username),
            new Claim(ClaimTypes.NameIdentifier, viewer.Id.ToString()),
            new Claim(ClaimTypes.Thumbprint, viewer.AvatarUrl),
            new Claim("RoomId", roomId.ToString())
        }, ApplicationConstants.RoomScheme)));
}