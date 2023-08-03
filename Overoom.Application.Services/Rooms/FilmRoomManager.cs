using Microsoft.Extensions.Caching.Memory;
using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Common.Exceptions;
using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.FilmRoom.Entities;

namespace Overoom.Application.Services.Rooms;

public class FilmRoomManager : IFilmRoomManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilmRoomMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public FilmRoomManager(IUnitOfWork unitOfWork, IFilmRoomMapper mapper, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public Task<(Guid roomId, int viewerId)> CreateAnonymouslyAsync(CreateFilmRoomDto dto, string name)
    {
        var viewer = new ViewerDto(name, ApplicationConstants.DefaultAvatar);
        var room = new FilmRoom(dto.FilmId, dto.CdnType, dto.IsOpen, viewer);
        return AddAsync(room);
    }

    public async Task<(Guid roomId, int viewerId)> CreateAsync(CreateFilmRoomDto dto, Guid userId)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        var viewer = new ViewerDto(user);
        var room = new FilmRoom(dto.FilmId, dto.CdnType, dto.IsOpen, viewer);
        return await AddAsync(room);
    }

    public async Task<int> ConnectAnonymouslyAsync(Guid roomId, string name)
    {
        var room = await GetRoomAsync(roomId);
        var viewer = new ViewerDto(name, ApplicationConstants.DefaultAvatar);
        var id = room.Connect(viewer);
        await SaveRoomAsync(room);
        return id;
    }

    public async Task<int> ConnectAsync(Guid roomId, Guid userId)
    {
        var room = await GetRoomAsync(roomId);
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        user.AddFilmToHistory(room.FilmId);
        await _unitOfWork.UserRepository.Value.UpdateAsync(user);
        var viewer = new ViewerDto(user);
        var id = room.Connect(viewer);
        await SaveRoomAsync(room);
        return id;
    }

    public async Task ChangeSeries(Guid roomId, int viewerId, int season, int series)
    {
        var room = await GetRoomAsync(roomId);
        room.ChangeSeason(viewerId, season);
        room.ChangeSeries(viewerId, series);
        await SaveRoomAsync(room);
    }

    public async Task SendMessageAsync(Guid roomId, int viewerId, string message)
    {
        var room = await GetRoomAsync(roomId);
        room.SendMessage(viewerId, message);
        await SaveRoomAsync(room);
    }

    public async Task PauseAsync(Guid roomId, int viewerId, bool pause)
    {
        var room = await GetRoomAsync(roomId);
        room.SetPause(viewerId, pause);
        await SaveRoomAsync(room);
    }

    public async Task FullScreenAsync(Guid roomId, int viewerId, bool fullScreen)
    {
        var room = await GetRoomAsync(roomId);
        room.SetFullScreen(viewerId, fullScreen);
        await SaveRoomAsync(room);
    }

    public async Task SeekAsync(Guid roomId, int viewerId, TimeSpan time)
    {
        var room = await GetRoomAsync(roomId);
        room.SetTimeLine(viewerId, time);
        await SaveRoomAsync(room);
    }

    public async Task DisconnectAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, false);
        await SaveRoomAsync(room);
    }

    public async Task ReConnectAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);
    }

    public async Task BeepAsync(Guid roomId, int viewerId, int target)
    {
        var room = await GetRoomAsync(roomId);
        room.Beep(viewerId, target);
    }

    public async Task ScreamAsync(Guid roomId, int viewerId, int target)
    {
        var room = await GetRoomAsync(roomId);
        room.Scream(viewerId, target);
    }

    public async Task KickAsync(Guid roomId, int viewerId, int target)
    {
        var room = await GetRoomAsync(roomId);
        room.Kick(viewerId, target);
        await SaveRoomAsync(room);
    }

    public async Task ChangeNameAsync(Guid roomId, int viewerId, int target, string name)
    {
        var room = await GetRoomAsync(roomId);
        room.ChangeName(viewerId, target, name);
        await SaveRoomAsync(room);
    }

    public async Task<FilmRoomDto> GetAsync(Guid roomId)
    {
        var room = await GetRoomAsync(roomId);
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        if (film == null) throw new FilmNotFoundException();
        return _mapper.Map(room, film);
    }

    public async Task<FilmViewerDto> GetAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        var viewer = room.Viewers.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        return _mapper.Map(viewer);
    }

    private async Task SaveRoomAsync(FilmRoom room)
    {
        await _unitOfWork.FilmRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<FilmRoom> GetRoomAsync(Guid id)
    {
        if (!_memoryCache.TryGetValue(id, out FilmRoom? room))
        {
            room = await _unitOfWork.FilmRoomRepository.Value.GetAsync(id);
            if (room == null) throw new FilmNotFoundException();
            _memoryCache.Set(id, room, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }
        else
        {
            if (room == null) throw new RoomNotFoundException();
        }

        return room;
    }


    private async Task<(Guid roomId, int viewerId)> AddAsync(FilmRoom room)
    {
        await _unitOfWork.FilmRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();
        _memoryCache.Set(room.Id, room, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        return (room.Id, room.Owner.Id);
    }
}