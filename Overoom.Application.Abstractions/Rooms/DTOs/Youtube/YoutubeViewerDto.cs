namespace Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

public class YoutubeViewerDto : ViewerDto
{
    public YoutubeViewerDto(string username, int id, Uri avatarUrl, TimeSpan time, bool onPause,
        string currentVideoId) : base(username, id, avatarUrl, time, onPause)
    {
        CurrentVideoId = currentVideoId;
    }

    public string CurrentVideoId { get; }
}