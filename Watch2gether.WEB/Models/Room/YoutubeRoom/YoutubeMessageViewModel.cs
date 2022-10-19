namespace Watch2gether.WEB.Models.Room.YoutubeRoom;

public class YoutubeMessageViewModel : MessageViewModel
{
    public new YoutubeViewerViewModel Viewer => (YoutubeViewerViewModel) base.Viewer;

    public YoutubeMessageViewModel(string text, DateTime createdAt, ViewerViewModel viewer) : base(text, createdAt, viewer)
    {
    }
}