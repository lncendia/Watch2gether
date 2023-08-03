namespace Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

public class YoutubeRoomDto : RoomDto
{
    public IReadOnlyCollection<string> Ids { get; }
    public bool AddAccess { get; }

    public new IReadOnlyCollection<YoutubeViewerDto> Viewers => base.Viewers.Cast<YoutubeViewerDto>().ToList();

    public YoutubeRoomDto(IReadOnlyCollection<string> ids, IReadOnlyCollection<MessageDto> messages,
        IReadOnlyCollection<YoutubeViewerDto> viewers, int ownerId, bool addAccess, bool isOpen) :
        base(messages, viewers, ownerId, isOpen)
    {
        Ids = ids;
        AddAccess = addAccess;
    }
}