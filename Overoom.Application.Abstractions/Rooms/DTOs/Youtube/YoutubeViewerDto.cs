namespace Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

public class YoutubeViewerDto : ViewerDto
{
    public YoutubeViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool pause, bool fullScreen,
        AllowsDto allows, string currentVideoId) : base(username, id, avatarUrl, time, pause, fullScreen, allows)
    {
        CurrentVideoId = currentVideoId;
    }

    public string CurrentVideoId { get; }
}