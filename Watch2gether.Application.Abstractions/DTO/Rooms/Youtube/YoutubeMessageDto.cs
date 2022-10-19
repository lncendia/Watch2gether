namespace Watch2gether.Application.Abstractions.DTO.Rooms.Youtube;

public class YoutubeMessageDto : MessageDto
{
    public new YoutubeViewerDto Viewer => (YoutubeViewerDto) base.Viewer;

    public YoutubeMessageDto(string text, DateTime createdAt, YoutubeViewerDto viewer) : base(text, createdAt, viewer)
    {
    }
}