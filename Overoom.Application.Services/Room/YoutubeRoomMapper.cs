using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;

namespace Overoom.Application.Services.Room;

public class YoutubeRoomMapper : IYoutubeRoomMapper
{
    public YoutubeRoomDto Map(YoutubeRoom room)
    {
        var viewers = room.Viewers.Where(x => x.Online).Select(Map).ToList();
        var messages = room.Messages
            .Select(m => new YoutubeMessageDto(m.Text, m.CreatedAt, viewers.First(x => x.Id == m.ViewerId))).ToList();
        return new YoutubeRoomDto(room.VideoIds, messages, viewers, room.Owner.Id, room.AddAccess);
    }

    public YoutubeViewerDto Map(YoutubeViewer v) =>
        new(v.Name, v.Id, v.AvatarUri, v.TimeLine, v.OnPause, v.CurrentVideoId);
}