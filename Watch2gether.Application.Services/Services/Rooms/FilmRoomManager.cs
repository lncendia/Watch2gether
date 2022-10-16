using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Films;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Rooms.BaseRoom.Entities;
using Watch2gether.Domain.Rooms.FilmRoom;
using Watch2gether.Domain.Users.Specifications;

namespace Watch2gether.Application.Services.Services.Rooms;

public class FilmRoomManager : IFilmRoomManager
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IFilmRoomService _roomService;

    public FilmRoomManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(Guid filmId, string name)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        return await CreateAsync(filmId, name, ApplicationConstants.DefaultAvatar);
    }


    public async Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(Guid filmId, string email)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(filmId, user.Name, user.AvatarFileName);
    }

    // public async Task ChangeSeason(Guid roomId, Guid viewerId, int season)
    // {
    //     var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
    //     if (room == null) throw new RoomNotFoundException();
    //     await _roomService.ChangeSeasonAsync(room, viewerId, season);
    //     await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
    //     await _unitOfWork.SaveAsync();
    // }
    //
    // public async Task ChangeSeries(Guid roomId, Guid viewerId, int series)
    // {
    //     var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
    //     if (room == null) throw new RoomNotFoundException();
    //     await _roomService.ChangeSeriesAsync(room, viewerId, series);
    //     await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
    //     await _unitOfWork.SaveAsync();
    // }

    public async Task<ViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return await ConnectAsync(room, name, ApplicationConstants.DefaultAvatar);
    }

    public async Task<ViewerDto> ConnectForUserAsync(Guid roomId, string email)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarFileName);
    }

    private async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(Guid filmId, string name, string avatarFileName)
    {
        var room = new FilmRoom(filmId, name, avatarFileName);
        await _unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, Map(room.Owner));
    }

    private async Task<ViewerDto> ConnectAsync(FilmRoom room, string name, string avatarFileName)
    {
        var viewer = room.Connect(name, avatarFileName);
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        return Map(viewer);
    }

    public async Task SendMessageAsync(Guid roomId, Guid viewerId, string message)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SendMessage(viewerId, message);
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task SetOnlineAsync(Guid roomId, Guid viewerId, bool online)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, online);
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }


    public async Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.UpdateTimeLine(viewerId, pause, time);
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> IsOwnerAsync(Guid roomId, Guid viewerId)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room.Owner.Id == viewerId;
    }

    public async Task<FilmRoomDto> GetAsync(Guid roomId, Guid viewerId)
    {
        var room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, true);
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        return Map(room, film!);
    }


    private static FilmRoomDto Map(FilmRoom room, Film film)
    {
        var viewers = room.Viewers;
        var messagesDtos = room.Messages
            .Select(m => new MessageDto(m.Text, m.CreatedAt, Map(viewers.First(x => x.Id == m.ViewerId)))).ToList();
        var viewersDtos = viewers.Where(x => x.Online).Select(Map).ToList();
        var filmDto = new FilmDataDto(film.Id, film.FilmData.Name, film.Url.AbsoluteUri);
        return new FilmRoomDto(filmDto, messagesDtos, viewersDtos, room.Owner.Id);
    }

    private static ViewerDto Map(Viewer v) => new(v.Name, v.Id, v.AvatarFileName, v.TimeLine, v.OnPause);
}