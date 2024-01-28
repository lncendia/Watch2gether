using Films.Application.Abstractions.Rooms.DTOs.Youtube;

namespace Films.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomMapper
{
    YoutubeRoomDto Map(YoutubeRoom room);

    YoutubeViewerDto Map(YoutubeViewer v);
}