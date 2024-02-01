using Films.Application.Abstractions.Rooms.DTOs;
using Films.Application.Abstractions.Rooms.DTOs.Youtube;
using Films.Application.Abstractions.Rooms.Interfaces;

namespace Films.Application.Services.Rooms;

public class YoutubeRoomMapper : IYoutubeRoomMapper
{
    public YoutubeRoomDto Map(YoutubeRoom room)
    {
        var viewersEntities = room.Viewers;
        var viewers = viewersEntities.Where(x => x.Online).Select(Map).ToList();
        var messages = room.Messages
            .Select(m =>
            {
                var viewer = viewersEntities.First(x => x.Id == m.ViewerId);
                return new MessageDto(m.Text, m.CreatedAt, viewer.Id, viewer.AvatarUrl, viewer.Name);
            }).ToList();
        return new YoutubeRoomDto(room.VideoIds, messages, viewers, room.Owner.Id, room.Access, room.IsOpen);
    }

    public YoutubeViewerDto Map(YoutubeViewer v)
    {
        var allows = new AllowsDto(v.Allows.Beep, v.Allows.Scream, v.Allows.Change);
        return new YoutubeViewerDto(v.Name, v.Id, v.AvatarUrl, v.TimeLine, v.Pause, v.FullScreen, allows,
            v.CurrentVideoId);
    }
}