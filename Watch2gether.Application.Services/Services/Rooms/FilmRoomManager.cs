using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms.Film;
using Watch2gether.Application.Abstractions.Exceptions.Films;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Rooms.FilmRoom;
using Watch2gether.Domain.Rooms.FilmRoom.Entities;
using Watch2gether.Domain.Users.Specifications;

namespace Watch2gether.Application.Services.Services.Rooms;

public class FilmRoomManager : IFilmRoomManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmRoomService _roomService;

    public FilmRoomManager(IUnitOfWork unitOfWork, IFilmRoomService roomService)
    {
        _unitOfWork = unitOfWork;
        _roomService = roomService;
    }

    public async Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, string name)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        return await CreateAsync(filmId, name, ApplicationConstants.DefaultAvatar);
    }


    public async Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, string email)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(filmId, user.Name, user.AvatarFileName);
    }

    public async Task ChangeSeries(Guid roomId, Guid viewerId, int season, int series)
    {
        var room = await GetRoomAsync(roomId);
        await _roomService.ChangeSeriesAsync(room, viewerId, season, series);
        await SaveRoomAsync(room);
    }

    public async Task<FilmViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await GetRoomAsync(roomId);
        return await ConnectAsync(room, name, ApplicationConstants.DefaultAvatar);
    }

    public async Task<FilmViewerDto> ConnectForUserAsync(Guid roomId, string email)
    {
        var room = await GetRoomAsync(roomId);
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarFileName);
    }

    private async Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, string name, string avatarFileName)
    {
        var room = new FilmRoom(filmId, name, avatarFileName);
        await _unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, Map(room.Owner));
    }

    private async Task<FilmViewerDto> ConnectAsync(FilmRoom room, string name, string avatarFileName)
    {
        var viewer = room.Connect(name, avatarFileName);
        await SaveRoomAsync(room);
        return Map(viewer);
    }

    public async Task SendMessageAsync(Guid roomId, Guid viewerId, string message)
    {
        var room = await GetRoomAsync(roomId);
        room.SendMessage(viewerId, message);
        await SaveRoomAsync(room);
    }

    public async Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time)
    {
        var room = await GetRoomAsync(roomId);
        room.UpdateTimeLine(viewerId, pause, time);
        await SaveRoomAsync(room);
    }

    public async Task DisconnectAsync(Guid roomId, Guid viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, false);
        await SaveRoomAsync(room);
    }

    public async Task<FilmViewerDto> ConnectAsync(Guid roomId, Guid viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);

        var viewer = room.Viewers.First(x => x.Id == viewerId);
        return Map(viewer);
    }

    public async Task<FilmRoomDto> GetAsync(Guid roomId, Guid viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        return Map(room, film!);
    }


    private static FilmRoomDto Map(FilmRoom room, Film film)
    {
        var viewers = room.Viewers;
        var messagesDtos = room.Messages
            .Select(m => new FilmMessageDto(m.Text, m.CreatedAt, Map(viewers.First(x => x.Id == m.ViewerId))));
        var viewersDtos = viewers.Where(x => x.Online).Select(Map);
        var filmDto = new FilmDataDto(film.Id, film.FilmData.Name, film.Url.AbsoluteUri, film.Type);
        return new FilmRoomDto(filmDto, messagesDtos, viewersDtos, room.Owner.Id);
    }

    private static FilmViewerDto Map(FilmViewer v) =>
        new(v.Name, v.Id, v.AvatarFileName, v.TimeLine, v.OnPause, v.Season, v.Series);

    private async Task<FilmRoom> GetRoomAsync(Guid roomId)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room;
    }

    private async Task SaveRoomAsync(FilmRoom room)
    {
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }
}