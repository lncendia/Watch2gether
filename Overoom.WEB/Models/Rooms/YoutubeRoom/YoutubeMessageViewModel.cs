namespace Overoom.WEB.Models.Rooms.YoutubeRoom;

public class YoutubeMessageViewModel : MessageViewModel
{
    public new YoutubeViewerViewModel Viewer => (YoutubeViewerViewModel) base.Viewer;

    public YoutubeMessageViewModel(string text, DateTime createdAt, YoutubeViewerViewModel viewer) : base(text, createdAt, viewer)
    {
    }
}