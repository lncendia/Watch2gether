using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Rooms;
using Overoom.WEB.Models.Rooms.YoutubeRoom;

namespace Overoom.WEB.Mappers;

public class YoutubeRoomMapper : IYoutubeRoomMapper
{
    public YoutubeRoomViewModel Map(YoutubeRoomDto dto, int id, string url)
    {
        var messages = dto.Messages.Select(Map).ToList();
        var viewers = dto.Viewers.Select(Map).ToList();
        return new YoutubeRoomViewModel(messages, viewers, url, dto.OwnerId, id, dto.Ids, dto.AddAccess);
    }

    private static YoutubeViewerViewModel Map(YoutubeViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.OnPause, dto.Time, dto.CurrentVideoId);

    private static MessageViewModel Map(MessageDto dto) => new(dto.Text, dto.CreatedAt, dto.ViewerId, dto.AvatarUri, dto.Username);
}