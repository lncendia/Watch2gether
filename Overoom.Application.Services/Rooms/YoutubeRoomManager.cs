using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.Application.Abstractions.Rooms.Exceptions;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.Users.Exceptions;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;

namespace Overoom.Application.Services.Rooms;

public class YoutubeRoomManager : IYoutubeRoomManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYoutubeRoomMapper _mapper;

    public YoutubeRoomManager(IUnitOfWork unitOfWork, IYoutubeRoomMapper youtubeRoomMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = youtubeRoomMapper;
    }

    public async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess) =>
        await CreateAsync(url, name, addAccess, ApplicationConstants.DefaultAvatar);


    public async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateForUserAsync(string url, Guid userId,
        bool addAccess)
    {
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(url, user.Name, addAccess, user.AvatarUri);
    }

    public async Task<string> AddVideoAsync(Guid roomId, int viewerId, string url)
    {
        var room = await GetRoomAsync(roomId);
        var id = room.AddVideo(viewerId, url);
        await SaveRoomAsync(room);
        return id;
    }

    public async Task RemoveVideoAsync(Guid roomId, string id)
    {
        var room = await GetRoomAsync(roomId);
        room.RemoveId(id);
        await SaveRoomAsync(room);
    }

    public async Task ChangeVideoAsync(Guid roomId, int viewerId, string id)
    {
        var room = await GetRoomAsync(roomId);
        room.ChangeVideo(viewerId, id);
        await SaveRoomAsync(room);
    }

    public async Task<YoutubeViewerDto> ConnectAsync(Guid roomId, int viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);
        var viewer = room.Viewers.First(x => x.Id == viewerId);
        return _mapper.Map(viewer);
    }

    public async Task<YoutubeViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await GetRoomAsync(roomId);
        return await ConnectAsync(room, name, ApplicationConstants.DefaultAvatar);
    }

    public async Task<YoutubeViewerDto> ConnectForUserAsync(Guid roomId, Guid userId)
    {
        var room = await GetRoomAsync(roomId);
        var user = await _unitOfWork.UserRepository.Value.GetAsync(userId);
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarUri);
    }

    private async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess,
        Uri avatarUri)
    {
        var room = new YoutubeRoom(url, name, avatarUri, addAccess);
        await _unitOfWork.YoutubeRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();
        return (room.Id, _mapper.Map(room.Owner));
    }

    private async Task<YoutubeViewerDto> ConnectAsync(YoutubeRoom room, string name, Uri avatarUri)
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

    public async Task<YoutubeRoomDto> GetAsync(Guid roomId)
    {
        var room = await GetRoomAsync(roomId);
        return _mapper.Map(room);
    }

    private async Task<YoutubeRoom> GetRoomAsync(Guid roomId)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room;
    }

    private async Task SaveRoomAsync(YoutubeRoom room)
    {
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();
    }
}