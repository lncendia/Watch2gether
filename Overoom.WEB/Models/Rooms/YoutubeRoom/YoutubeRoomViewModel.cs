namespace Overoom.WEB.Models.Rooms.YoutubeRoom;

public class YoutubeRoomViewModel : BaseRoomViewModel
{
    public YoutubeRoomViewModel(IReadOnlyCollection<MessageViewModel> messages,
        IReadOnlyCollection<YoutubeViewerViewModel> viewers, string connectUrl, int ownerId, int currentViewerId,
        IReadOnlyCollection<string> ids, bool access, bool isOpen) : base(messages, viewers, connectUrl, ownerId,
        currentViewerId, isOpen)
    {
        Ids = ids;
        Access = access;
    }

    public IReadOnlyCollection<string> Ids { get; }
    public bool Access { get; }

    public new IReadOnlyCollection<YoutubeViewerViewModel> Viewers =>
        base.Viewers.Cast<YoutubeViewerViewModel>().ToList();
}