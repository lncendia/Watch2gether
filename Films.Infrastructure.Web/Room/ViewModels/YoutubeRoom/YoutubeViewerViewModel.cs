namespace Films.Infrastructure.Web.Models.Rooms.YoutubeRoom;

public class YoutubeViewerViewModel : ViewerViewModel
{
    public string CurrentVideoId { get; }

    public YoutubeViewerViewModel(int id, string username, Uri avatarUrl, bool pause, TimeSpan time,
        string currentVideoId, bool fullScreen, bool allowBeep, bool allowScream, bool allowChange) : base(id, username,
        avatarUrl, pause, time, fullScreen, allowBeep, allowScream, allowChange)
    {
        CurrentVideoId = currentVideoId;
    }
}