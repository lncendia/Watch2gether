namespace Watch2gether.Application.Abstractions.DTO.Rooms;

public class YoutubeRoomDto : RoomDto
{
    public List<string> Ids { get; }

    public YoutubeRoomDto(List<string> ids, List<MessageDto> messages, List<ViewerDto> viewers, Guid ownerId) : base(
        messages, viewers, ownerId)
    {
        Ids = ids;
    }
}