using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;

namespace Overoom.Application.Services.Rooms;

public class YoutubeRoomMapper : IYoutubeRoomMapper
{
    public YoutubeRoomDto Map(YoutubeRoom room)
    {
        var viewers = room.Viewers.Where(x => x.Online).Select(Map).ToList();
        var messages = room.Messages
            .Select(m =>
            {
                var viewer = viewers.First(x => x.Id == m.ViewerId);
                return new MessageDto(m.Text, m.CreatedAt, viewer.Id, viewer.AvatarUrl, viewer.Username);
            }).ToList();
        return new YoutubeRoomDto(room.VideoIds, messages, viewers, room.Owner.Id, room.AddAccess);
    }

    public YoutubeViewerDto Map(YoutubeViewer v) =>
        new(v.Name, v.Id, v.AvatarUri, v.TimeLine, v.OnPause, v.CurrentVideoId);
}