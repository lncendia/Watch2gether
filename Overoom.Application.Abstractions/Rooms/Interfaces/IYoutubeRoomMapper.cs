using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomMapper
{
    YoutubeRoomDto Map(YoutubeRoom room);

    YoutubeViewerDto Map(YoutubeViewer v);
}