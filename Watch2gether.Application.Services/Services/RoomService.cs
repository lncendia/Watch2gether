using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Rooms.Entities;
using Watch2gether.Domain.Rooms.Exceptions;
using Watch2gether.Domain.Users.Specifications;

namespace Watch2gether.Application.Services.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private const string DefaultAvatar = "default.jpg";

    public RoomService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(Guid filmId, string name)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new ArgumentException("Film not found.", nameof(filmId));
        return await CreateAsync(filmId, name, DefaultAvatar);
    }


    public async Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(Guid filmId, string email)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(filmId);
        if (film == null) throw new ArgumentException("Film not found.", nameof(filmId)); //TODO: add exception
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserFromEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(filmId, user.Name, user.AvatarFileName);
    }

    public async Task<ViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return await ConnectAsync(room, name, DefaultAvatar);
    }

    public async Task<ViewerDto> ConnectForUserAsync(Guid roomId, string email)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserFromEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarFileName);
    }

    private async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(Guid filmId, string name, string avatarFileName)
    {
        var room = new Room(filmId, name, avatarFileName);
        await _unitOfWork.RoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, Map(room.Owner));
    }

    private async Task<ViewerDto> ConnectAsync(Room room, string name, string avatarFileName)
    {
        var viewer = room.Connect(name, avatarFileName);
        await _unitOfWork.RoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        return Map(viewer);
    }

    public async Task SendMessageAsync(Guid roomId, Guid viewerId, string message)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SendMessage(viewerId, message);
        await _unitOfWork.RoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task SetOnlineAsync(Guid roomId, Guid viewerId, bool online)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, online);
        await _unitOfWork.RoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }


    public async Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.UpdateViewer(viewerId, pause, time);
        await _unitOfWork.RoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> IsOwnerAsync(Guid roomId, Guid viewerId)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room.Owner.Id == viewerId;
    }

    public async Task<RoomDto> GetAsync(Guid roomId, Guid viewerId)
    {
        var room = await _unitOfWork.RoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, true);
        await _unitOfWork.RoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        return Map(room, film!);
    }


    private static RoomDto Map(Room room, Film film)
    {
        var viewers = room.Viewers;
        var messagesDtos = room.Messages
            .Select(m => new MessageDto(m.Text, m.CreatedAt, Map(viewers.First(x => x.Id == m.ViewerId)))).ToList();
        var viewersDtos = viewers.Where(x => x.Online).Select(Map).ToList();
        var filmDto = new FilmDataDto(film.Id, film.FilmData.Name, film.Url.AbsoluteUri);
        return new RoomDto(messagesDtos, filmDto, viewersDtos, room.Owner.Id);
    }

    private static ViewerDto Map(Viewer v) => new(v.Name, v.Id, v.AvatarFileName, v.TimeLine, v.OnPause);
}