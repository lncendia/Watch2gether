namespace Overoom.WEB.Models.Rooms.YoutubeRoom;

public class YoutubeViewerViewModel : ViewerViewModel
{
    public string CurrentVideoId { get; }
    
    public YoutubeViewerViewModel(int id, string username, Uri avatarUrl, bool onPause, TimeSpan time,
        string currentVideoId) : base(id, username, avatarUrl, onPause, time)
    {
        CurrentVideoId = currentVideoId;
    }
}