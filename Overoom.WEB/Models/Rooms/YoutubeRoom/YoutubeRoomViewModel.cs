namespace Overoom.WEB.Models.Rooms.YoutubeRoom;

public class YoutubeRoomViewModel : BaseRoomViewModel
{
    public YoutubeRoomViewModel(IEnumerable<YoutubeMessageViewModel> messages,
        IEnumerable<YoutubeViewerViewModel> viewers, string connectUrl,
        Guid ownerId, Guid currentViewerId, List<string> ids, bool addAccess) : base(messages, viewers, connectUrl, ownerId,
        currentViewerId)
    {
        Ids = ids;
        AddAccess = addAccess;
    }

    public List<string> Ids { get; }
    public bool AddAccess { get; }
    public new List<YoutubeViewerViewModel> Viewers => base.Viewers.Cast<YoutubeViewerViewModel>().ToList();
    public new List<YoutubeMessageViewModel> Messages => base.Messages.Cast<YoutubeMessageViewModel>().ToList();
    //public new YoutubeViewerViewModel CurrentViewer => (YoutubeViewerViewModel) base.CurrentViewer;
}