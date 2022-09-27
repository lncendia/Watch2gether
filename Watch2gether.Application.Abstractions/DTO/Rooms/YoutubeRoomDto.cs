namespace Watch2gether.Application.Abstractions.DTO.Rooms;

public class YoutubeRoomDto : RoomDto
{
    public string Link { get; }

    public YoutubeRoomDto(string link, List<MessageDto> messages, List<ViewerDto> viewers, Guid ownerId) : base(
        messages, viewers, ownerId)
    {
        Link = link;
    }
}