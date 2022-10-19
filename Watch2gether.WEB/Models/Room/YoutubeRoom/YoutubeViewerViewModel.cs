namespace Watch2gether.WEB.Models.Room.YoutubeRoom;

public class YoutubeViewerViewModel : ViewerViewModel
{
    public string CurrentVideoId { get; }
    
    public YoutubeViewerViewModel(Guid id, string username, string avatarUrl, bool onPause, TimeSpan time,
        string currentVideoId) : base(id, username, avatarUrl, onPause, time)
    {
        CurrentVideoId = currentVideoId;
    }
}