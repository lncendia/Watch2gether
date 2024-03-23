using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.YoutubeRooms;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Rooms.InputModels;
using Films.Infrastructure.Web.Rooms.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ConnectRoomCommand = Films.Application.Abstractions.Commands.YoutubeRooms.ConnectRoomCommand;
using CreateRoomCommand = Films.Application.Abstractions.Commands.YoutubeRooms.CreateRoomCommand;

namespace Films.Infrastructure.Web.Rooms.Controllers;

[ApiController]
[Route("filmApi/[controller]/[action]")]
public class YoutubeRoomsController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<RoomServerViewModel> Create(CreateYoutubeRoomInputModel model)
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
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/'),
            Code = serverRoom.Code
        };
    }

    [HttpPost]
    public async Task<RoomServerViewModel> Connect(ConnectRoomInputModel model)
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
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/'),
            Code = serverRoom.Code
        };
    }

    [HttpGet]
    public async Task<RoomsViewModel<YoutubeRoomViewModel>> Search([FromQuery] RoomSearchInputModel model)
    {
        var data = await mediator.Send(new SearchYoutubeRoomsQuery
        {
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage,
            UserId = User.Identity?.IsAuthenticated ?? false ? User.GetId() : null,
            OnlyPublic = model.OnlyPublic,
            OnlyMy = model.OnlyMy
        });

        var count = data.count / model.CountPerPage;
        if (data.count % model.CountPerPage > 0) count++;
        return new RoomsViewModel<YoutubeRoomViewModel>
        {
            CountPages = count,
            Rooms = data.rooms.Select(Map)
        };
    }


    [HttpGet("{id:guid}")]
    public async Task<YoutubeRoomViewModel> Room(Guid id)
    {
        var room = await mediator.Send(new YoutubeRoomByIdQuery
        {
            Id = id,
            UserId = User.Identity?.IsAuthenticated ?? false ? User.GetId() : null
        });
        return Map(room);
    }

    private static YoutubeRoomViewModel Map(YoutubeRoomDto dto) => new()
    {
        VideoAccess = dto.VideoAccess,
        Id = dto.Id,
        ViewersCount = dto.ViewersCount,
        IsCodeNeeded = dto.IsCodeNeeded,
    };
}