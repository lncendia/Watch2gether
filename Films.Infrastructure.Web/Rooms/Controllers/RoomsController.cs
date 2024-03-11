using Films.Application.Abstractions.Commands.Rooms.FilmRooms;
using Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;
using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.Rooms;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Rooms.InputModels;
using Films.Infrastructure.Web.Rooms.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ConnectRoomCommand = Films.Application.Abstractions.Commands.Rooms.YoutubeRooms.ConnectRoomCommand;
using CreateRoomCommand = Films.Application.Abstractions.Commands.Rooms.YoutubeRooms.CreateRoomCommand;

namespace Films.Infrastructure.Web.Rooms.Controllers;

[ApiController]
[Route("filmApi/[controller]/[action]")]
public class RoomsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<RoomServerViewModel> CreateFilmRoom(CreateFilmRoomInputModel model)
    {
        var serverRoom = await mediator.Send(new Application.Abstractions.Commands.Rooms.FilmRooms.CreateRoomCommand
        {
            UserId = User.GetId(),
            IsOpen = model.Open,
            FilmId = model.FilmId,
            CdnName = model.CdnName!
        });

        return new RoomServerViewModel
        {
            Id = serverRoom.Id,
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/')
        };
    }

    [HttpPost]
    public async Task<RoomServerViewModel> CreateYoutubeRoom(CreateYoutubeRoomInputModel model)
    {
        var serverRoom = await mediator.Send(new CreateRoomCommand
        {
            UserId = User.GetId(),
            IsOpen = model.Open,
            VideoAccess = model.VideoAccess
        });

        return new RoomServerViewModel
        {
            Id = serverRoom.Id,
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/')
        };
    }

    [HttpPost]
    public async Task<RoomServerViewModel> ConnectFilmRoom(ConnectRoomInputModel model)
    {
        var serverRoom = await mediator.Send(new Application.Abstractions.Commands.Rooms.FilmRooms.ConnectRoomCommand
        {
            UserId = User.GetId(),
            RoomId = model.Id,
            Code = model.Code
        });

        return new RoomServerViewModel
        {
            Id = serverRoom.Id,
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/')
        };
    }

    [HttpPost]
    public async Task<RoomServerViewModel> ConnectYoutubeRoom(ConnectRoomInputModel model)
    {
        var serverRoom = await mediator.Send(new ConnectRoomCommand
        {
            UserId = User.GetId(),
            RoomId = model.Id,
            Code = model.Code
        });

        return new RoomServerViewModel
        {
            Id = serverRoom.Id,
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/')
        };
    }

    [HttpGet]
    public async Task<IEnumerable<FilmRoomViewModel>> UserFilmRooms()
    {
        var rooms = await mediator.Send(new UserFilmRoomsQuery { Id = User.GetId() });
        return rooms.Select(Map);
    }

    [HttpGet]
    public async Task<IEnumerable<YoutubeRoomViewModel>> UserYoutubeRooms()
    {
        var rooms = await mediator.Send(new UserYoutubeRoomsQuery { Id = User.GetId() });
        return rooms.Select(Map);
    }


    [HttpGet("{id:guid}")]
    public async Task<FilmRoomViewModel> FilmRoom(Guid id)
    {
        var room = await mediator.Send(new FilmRoomByIdQuery { Id = id });
        return Map(room);
    }

    [HttpGet("{id:guid}")]
    public async Task<YoutubeRoomViewModel> YoutubeRoom(Guid id)
    {
        var room = await mediator.Send(new YoutubeRoomByIdQuery { Id = id });
        return Map(room);
    }


    private static FilmRoomViewModel Map(FilmRoomDto dto) => new()
    {
        Title = dto.Title,
        PosterUrl = dto.PosterUrl.ToString().Replace('\\', '/'),
        Year = dto.Year,
        UserRating = dto.UserRating,
        Description = dto.Description,
        IsSerial = dto.IsSerial,
        Id = dto.Id,
        ViewersCount = dto.ViewersCount,
        ServerUrl = dto.ServerUrl.ToString().Replace('\\', '/'),
        IsClosed = dto.IsClosed
    };

    private static YoutubeRoomViewModel Map(YoutubeRoomDto dto) => new()
    {
        VideoAccess = dto.VideoAccess,
        Id = dto.Id,
        ViewersCount = dto.ViewersCount,
        ServerUrl = dto.ServerUrl.ToString().Replace('\\', '/'),
        IsClosed = dto.IsClosed
    };
}