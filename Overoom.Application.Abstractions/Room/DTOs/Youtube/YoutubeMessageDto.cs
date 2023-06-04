namespace Overoom.Application.Abstractions.Room.DTOs.Youtube;

public class YoutubeMessageDto : MessageDto
{
    public new YoutubeViewerDto Viewer => (YoutubeViewerDto) base.Viewer;

    public YoutubeMessageDto(string text, DateTime createdAt, YoutubeViewerDto viewer) : base(text, createdAt, viewer)
    {
    }
}