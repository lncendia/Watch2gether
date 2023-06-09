using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Film.Exceptions;
using Overoom.Application.Abstractions.Room.DTOs.Film;
using Overoom.Application.Abstractions.Room.Exceptions;
using Overoom.Application.Abstractions.Room.Interfaces;
using Overoom.Application.Abstractions.User.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Film.Enums;
using Overoom.Domain.Room.FilmRoom.Entities;
using Overoom.Domain.User.Specifications;

namespace Overoom.Application.Services.Room;

public class FilmRoomManager : IFilmRoomManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmRoomMapper _mapper;

    protected FilmRoomManager(IUnitOfWork unitOfWork, IFilmRoomMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, CdnType cdn, string name)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        return await CreateAsync(filmId, cdn, name, ApplicationConstants.DefaultAvatar);
    }


    public async Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, CdnType cdn, Guid userId)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new FilmNotFoundException();
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        user.History.Add(filmId);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        return await CreateAsync(filmId, cdn, user.Name, user.AvatarUri);
    }

    public async Task ChangeSeries(Guid roomId, int viewerId, int season, int series)
    {
        var room = await GetRoomAsync(roomId);
        room.ChangeSeason(viewerId, season);
        room.ChangeSeries(viewerId, series);
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
        user.History.Add(room.FilmId);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        return await ConnectAsync(room, user.Name, user.AvatarUri);
    }

    private async Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, CdnType cdn, string name,
        string avatarUri)
    {
        var room = new FilmRoom(filmId, name, avatarUri, cdn);
        await _unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, _mapper.Map(room.Owner));
    }

    private async Task<FilmViewerDto> ConnectAsync(FilmRoom room, string name, string avatarUri)
    {
        var viewer = room.Connect(name, avatarUri);
        await SaveRoomAsync(room);
        return _mapper.Map(viewer);
    }

    public async Task SendMessageAsync(Guid roomId, int viewerId, string message)
    {
        var room = await GetRoomAsync(roomId);
        room.SendMessage(viewerId, message);
        await SaveRoomAsync(room);
    }

    public async Task SetPauseAsync(Guid roomId, int viewerId, bool pause, TimeSpan time)
    {
        var room = await GetRoomAsync(roomId);
        room.UpdateTimeLine(viewerId, pause, time);
        await SaveRoomAsync(room);
    }

    public async Task DisconnectAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, false);
        await SaveRoomAsync(room);
    }

    public async Task<FilmViewerDto> ConnectAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);

        var viewer = room.Viewers.First(x => x.Id == viewerId);
        return _mapper.Map(viewer);
    }

    public async Task<FilmRoomDto> GetAsync(Guid roomId)
    {
        var room = await GetRoomAsync(roomId);
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        if (film == null) throw new FilmNotFoundException();
        return _mapper.Map(room, film);
    }

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