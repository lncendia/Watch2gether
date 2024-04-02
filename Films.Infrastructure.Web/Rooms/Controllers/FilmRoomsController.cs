using Films.Application.Abstractions.Commands.FilmRooms;
using Films.Application.Abstractions.DTOs.Rooms;
using Films.Application.Abstractions.Queries.FilmRooms;
using Films.Infrastructure.Web.Authentication;
using Films.Infrastructure.Web.Common.ViewModels;
using Films.Infrastructure.Web.Rooms.InputModels;
using Films.Infrastructure.Web.Rooms.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.Rooms.Controllers;

[ApiController]
[Route("filmApi/[controller]/[action]")]
public class FilmRoomsController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPut]
    public async Task<RoomServerViewModel> Create(CreateFilmRoomInputModel model)
    {
        var serverRoom = await mediator.Send(new CreateRoomCommand
        {
            UserId = User.GetId(),
            IsOpen = model.Open,
            FilmId = model.FilmId,
            CdnName = model.CdnName!
        });

        return new RoomServerViewModel
        {
            Id = serverRoom.Id,
            Url = serverRoom.ServerUrl.ToString().Replace('\\', '/'),
            Code = serverRoom.Code
        };
    }

    [Authorize]
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
    public async Task<ListViewModel<FilmRoomShortViewModel>> Search([FromQuery] FilmRoomSearchInputModel model)
    {
        var data = await mediator.Send(new SearchFilmRoomsQuery
        {
            Skip = (model.Page - 1) * model.CountPerPage,
            Take = model.CountPerPage,
            FilmId = model.FilmId,
            OnlyPublic = model.OnlyPublic,
        });

        var count = data.TotalCount / model.CountPerPage;
        if (data.TotalCount % model.CountPerPage > 0) count++;
        return new ListViewModel<FilmRoomShortViewModel>
        {
            TotalPages = count,
            List = data.List.Select(Map)
        };
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<FilmRoomShortViewModel>> My()
    {
        var rooms = await mediator.Send(new UserFilmRoomsQuery { UserId = User.GetId() });
        return rooms.Select(Map);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<FilmRoomViewModel> Room(Guid id)
    {
        var room = await mediator.Send(new FilmRoomByIdQuery
        {
            Id = id,
            UserId = User.Identity?.IsAuthenticated ?? false ? User.GetId() : null
        });
        return Map(room);
    }


    private FilmRoomShortViewModel Map(FilmRoomShortDto dto) => new()
    {
        Title = dto.Title,
        PosterUrl = $"{Request.Scheme}://{Request.Host}/{dto.PosterUrl.ToString().Replace('\\', '/')}",
        Year = dto.Year,
        UserRating = dto.UserRating,
        Description = dto.Description,
        IsSerial = dto.IsSerial,
        RatingKp = dto.RatingKp,
        RatingImdb = dto.RatingImdb,
        Id = dto.Id,
        ViewersCount = dto.ViewersCount,
        IsPrivate = dto.IsPrivate,
        FilmId = dto.FilmId,
    };

    private FilmRoomViewModel Map(FilmRoomDto dto) => new()
    {
        Title = dto.Title,
        PosterUrl = $"{Request.Scheme}://{Request.Host}/{dto.PosterUrl.ToString().Replace('\\', '/')}",
        Year = dto.Year,
        UserRating = dto.UserRating,
        Description = dto.Description,
        IsSerial = dto.IsSerial,
        RatingKp = dto.RatingKp,
        RatingImdb = dto.RatingImdb,
        Id = dto.Id,
        ViewersCount = dto.ViewersCount,
        IsPrivate = dto.IsPrivate,
        IsCodeNeeded = dto.IsCodeNeeded,
        FilmId = dto.FilmId,
        UserRatingsCount = dto.UserRatingsCount,
        UserScore = dto.UserScore
    };
}