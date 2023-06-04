namespace Overoom.Application.Abstractions.Room.DTOs.Youtube;

public class YoutubeViewerDto : ViewerDto
{
    public YoutubeViewerDto(string username, int id, string avatarUrl, TimeSpan time, bool onPause,
        string currentVideoId) : base(username, id, avatarUrl, time, onPause)
    {
        CurrentVideoId = currentVideoId;
    }

    public string CurrentVideoId { get; }
}