using Films.Application.Abstractions.Rooms.DTOs.Youtube;
using Films.Infrastructure.Web.Models.Rooms.YoutubeRoom;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IYoutubeRoomMapper
{
    YoutubeRoomViewModel Map(YoutubeRoomDto dto, int id, string url);
}