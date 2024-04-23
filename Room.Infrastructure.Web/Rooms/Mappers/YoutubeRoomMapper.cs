using Room.Application.Abstractions.DTOs.YoutubeRooms;
using Room.Infrastructure.Web.Rooms.ViewModels;
using Room.Infrastructure.Web.Rooms.ViewModels.Common;
using Room.Infrastructure.Web.Rooms.ViewModels.YoutubeRooms;

namespace Room.Infrastructure.Web.Rooms.Mappers;

public class YoutubeRoomMapper
{
    public static YoutubeRoomViewModel Map(YoutubeRoomDto dto) => new()
    {
        Id = dto.Id,
        OwnerId = dto.OwnerId,
        VideoAccess = dto.VideoAccess,
        Viewers = dto.Viewers.Select(v => new YoutubeViewerViewModel
            {
                Id = v.Id,
                Username = v.Username,
                PhotoUrl = v.PhotoUrl?.ToString(),
                Pause = v.Pause,
                FullScreen = v.FullScreen,
                Online = v.Online,
                Second = v.TimeLine.Seconds,
                VideoId = v.VideoId,
                Allows = new AllowsViewModel
                {
                    Beep = v.Allows.Beep,
                    Scream = v.Allows.Scream,
                    Change = v.Allows.Change
                }
            })
            .ToArray(),
        Videos = dto.Videos.Select(v => v.Id).ToArray(),
    };
}