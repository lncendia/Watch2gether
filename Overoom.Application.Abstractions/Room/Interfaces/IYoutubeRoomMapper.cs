using Overoom.Application.Abstractions.Room.DTOs.Youtube;
using Overoom.Domain.Room.YoutubeRoom.Entities;

namespace Overoom.Application.Abstractions.Room.Interfaces;

public interface IYoutubeRoomMapper
{
    YoutubeRoomDto Map(YoutubeRoom room);

    YoutubeViewerDto Map(YoutubeViewer v);
}