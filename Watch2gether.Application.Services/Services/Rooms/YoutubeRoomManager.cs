using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms.Youtube;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Rooms.YoutubeRoom;
using Watch2gether.Domain.Rooms.YoutubeRoom.Entities;
using Watch2gether.Domain.Users.Specifications;

namespace Watch2gether.Application.Services.Services.Rooms;

public class YoutubeRoomManager : IYoutubeRoomManager
{
    private readonly IUnitOfWork _unitOfWork;

    public YoutubeRoomManager(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess) =>
        await CreateAsync(url, name, addAccess, ApplicationConstants.DefaultAvatar);


    public async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateForUserAsync(string url, string email, bool addAccess)
    {
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(url, user.Name, addAccess, user.AvatarFileName);
    }

    public async Task<string> AddVideoAsync(Guid roomId, Guid viewerId, string url)
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

    public async Task ChangeVideoAsync(Guid roomId, Guid viewerId, string id)
    {
        var room = await GetRoomAsync(roomId);
        room.ChangeVideo(viewerId, id);
        await SaveRoomAsync(room);
    }

    public async Task<YoutubeViewerDto> ConnectAsync(Guid roomId, Guid viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);
        var viewer = room.Viewers.First(x => x.Id == viewerId);
        return Map(viewer);
    }

    public async Task<YoutubeViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await GetRoomAsync(roomId);
        return await ConnectAsync(room, name, ApplicationConstants.DefaultAvatar);
    }

    public async Task<YoutubeViewerDto> ConnectForUserAsync(Guid roomId, string email)
    {
        var room = await GetRoomAsync(roomId);
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarFileName);
    }

    private async Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess,
        string avatarFileName)
    {
        var room = new YoutubeRoom(url, name, avatarFileName, addAccess);
        await _unitOfWork.YoutubeRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, Map(room.Owner));
    }

    private async Task<YoutubeViewerDto> ConnectAsync(YoutubeRoom room, string name, string avatarFileName)
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

    public async Task<YoutubeRoomDto> GetAsync(Guid roomId, Guid viewerId)
    {
        var room = await GetRoomAsync(roomId);
        room.SetOnline(viewerId, true);
        await SaveRoomAsync(room);
        return Map(room);
    }


    private static YoutubeRoomDto Map(YoutubeRoom room)
    {
        var viewers = room.Viewers;
        var messagesDtos = room.Messages
            .Select(m => new YoutubeMessageDto(m.Text, m.CreatedAt, Map(viewers.First(x => x.Id == m.ViewerId))));
        var viewersDtos = viewers.Where(x => x.Online).Select(Map);
        return new YoutubeRoomDto(room.VideoIds, messagesDtos, viewersDtos, room.Owner.Id, room.AddAccess);
    }

    private static YoutubeViewerDto Map(YoutubeViewer v) =>
        new(v.Name, v.Id, v.AvatarFileName, v.TimeLine, v.OnPause, v.CurrentVideoId);
    
    private async Task<YoutubeRoom> GetRoomAsync(Guid roomId)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room;
    }

    private async Task SaveRoomAsync(YoutubeRoom room)
    {
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }
}