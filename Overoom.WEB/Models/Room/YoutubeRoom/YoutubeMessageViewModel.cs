namespace Overoom.WEB.Models.Room.YoutubeRoom;

public class YoutubeMessageViewModel : MessageViewModel
{
    public new YoutubeViewerViewModel Viewer => (YoutubeViewerViewModel) base.Viewer;

    public YoutubeMessageViewModel(string text, DateTime createdAt, YoutubeViewerViewModel viewer) : base(text, createdAt, viewer)
    {
    }
}