namespace Overoom.Application.Abstractions.DTO.Rooms.Youtube;

public class YoutubeRoomDto : RoomDto
{
    public List<string> Ids { get; }
    public bool AddAccess { get; }

    public new List<YoutubeViewerDto> Viewers => base.Viewers.Cast<YoutubeViewerDto>().ToList();
    public new List<YoutubeMessageDto> Messages => base.Messages.Cast<YoutubeMessageDto>().ToList();

    public YoutubeRoomDto(List<string> ids, IEnumerable<YoutubeMessageDto> messages, IEnumerable<YoutubeViewerDto> viewers,
        Guid ownerId, bool addAccess) : base(messages, viewers, ownerId)
    {
        Ids = ids;
        AddAccess = addAccess;
    }
}