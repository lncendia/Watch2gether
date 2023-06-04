namespace Overoom.Application.Abstractions.Room.DTOs.Youtube;

public class YoutubeRoomDto : RoomDto
{
    public IReadOnlyCollection<string> Ids { get; }
    public bool AddAccess { get; }

    public new IReadOnlyCollection<YoutubeViewerDto> Viewers => base.Viewers.Cast<YoutubeViewerDto>().ToList();
    public new IReadOnlyCollection<YoutubeMessageDto> Messages => base.Messages.Cast<YoutubeMessageDto>().ToList();

    public YoutubeRoomDto(IReadOnlyCollection<string> ids, IReadOnlyCollection<YoutubeMessageDto> messages, IReadOnlyCollection<YoutubeViewerDto> viewers,
        int ownerId, bool addAccess) : base(messages, viewers, ownerId)
    {
        Ids = ids;
        AddAccess = addAccess;
    }
}