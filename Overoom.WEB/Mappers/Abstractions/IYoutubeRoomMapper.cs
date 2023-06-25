using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.WEB.Models.Rooms.YoutubeRoom;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IYoutubeRoomMapper
{
    YoutubeRoomViewModel Map(YoutubeRoomDto dto, int id, string url);
}