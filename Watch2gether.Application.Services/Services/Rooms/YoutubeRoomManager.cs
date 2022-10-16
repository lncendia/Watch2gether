using Watch2gether.Application.Abstractions;
using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Rooms;
using Watch2gether.Application.Abstractions.Exceptions.Users;
using Watch2gether.Application.Abstractions.Interfaces.Rooms;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Rooms.BaseRoom.Entities;
using Watch2gether.Domain.Rooms.YoutubeRoom;
using Watch2gether.Domain.Users.Specifications;

namespace Watch2gether.Application.Services.Services.Rooms;

public class YoutubeRoomManager : IYoutubeRoomManager
{
    private readonly IUnitOfWork _unitOfWork;

    public YoutubeRoomManager(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(string url, string name) =>
        await CreateAsync(GetId(url), name, ApplicationConstants.DefaultAvatar);


    public async Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(string url, string email)
    {
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await CreateAsync(GetId(url), user.Name, user.AvatarFileName);
    }

    public async Task<string> AddVideoAsync(Guid roomId, string url)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        var id = GetId(url);
        room.AddId(id);
        return id;
    }

    public async Task RemoveVideoAsync(Guid roomId, string id)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.RemoveId(id);
    }

    public async Task<ViewerDto> ConnectAsync(Guid roomId, string name)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        return await ConnectAsync(room, name, ApplicationConstants.DefaultAvatar);
    }

    public async Task<ViewerDto> ConnectForUserAsync(Guid roomId, string email)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        var user = (await _unitOfWork.UserRepository.Value.FindAsync(new UserByEmailSpecification(email), null, 0, 1))
            .FirstOrDefault();
        if (user == null) throw new UserNotFoundException();
        return await ConnectAsync(room, user.Name, user.AvatarFileName);
    }

    private async Task<(Guid roomId, ViewerDto viewer)> CreateAsync(string url, string name, string avatarFileName)
    {
        var room = new YoutubeRoom(url, name, avatarFileName);
        await _unitOfWork.YoutubeRoomRepository.Value.AddAsync(room);
        await _unitOfWork.SaveAsync();
        return (room.Id, Map(room.Owner));
    }

    private async Task<ViewerDto> ConnectAsync(YoutubeRoom room, string name, string avatarFileName)
    {
        var viewer = room.Connect(name, avatarFileName);
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        return Map(viewer);
    }

    public async Task SendMessageAsync(Guid roomId, Guid viewerId, string message)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SendMessage(viewerId, message);
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task SetOnlineAsync(Guid roomId, Guid viewerId, bool online)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, online);
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }


    public async Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.UpdateTimeLine(viewerId, pause, time);
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task<YoutubeRoomDto> GetAsync(Guid roomId, Guid viewerId)
    {
        var room = await _unitOfWork.YoutubeRoomRepository.Value.GetAsync(roomId);
        if (room == null) throw new RoomNotFoundException();
        room.SetOnline(viewerId, true);
        await _unitOfWork.YoutubeRoomRepository.Value.UpdateAsync(room);
        await _unitOfWork.SaveAsync();
        return Map(room);
    }


    private static YoutubeRoomDto Map(YoutubeRoom room)
    {
        var viewers = room.Viewers;
        var messagesDtos = room.Messages
            .Select(m => new MessageDto(m.Text, m.CreatedAt, Map(viewers.First(x => x.Id == m.ViewerId)))).ToList();
        var viewersDtos = viewers.Where(x => x.Online).Select(Map).ToList();
        return new YoutubeRoomDto(room.VideoIds, messagesDtos, viewersDtos, room.Owner.Id);
    }

    private static ViewerDto Map(Viewer v) => new(v.Name, v.Id, v.AvatarFileName, v.TimeLine, v.OnPause);

    private static string GetId(string url)
    {
        var uri = new Uri(url);
        string id;
        try
        {
            id = uri.Host switch
            {
                "www.youtube.com" => uri.Query[..3],
                "youtu.be" => uri.Segments[1],
                _ => string.Empty
            };
        }
        catch
        {
            throw new InvalidVideoUrlException();
        }

        if (string.IsNullOrEmpty(id)) throw new InvalidVideoUrlException();
        return id;
    }
}