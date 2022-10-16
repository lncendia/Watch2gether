namespace Watch2gether.WEB.Models.Room.YoutubeRoom;

public class YoutubeRoomViewModel : BaseRoomViewModel
{
    public YoutubeRoomViewModel(List<MessageViewModel> messages, List<ViewerViewModel> viewers, string connectUrl,
        Guid ownerId, ViewerViewModel currentViewer, List<string> ids) : base(messages, viewers, connectUrl, ownerId,
        currentViewer)
    {
        Ids = ids;
    }

    public List<string> Ids { get; }
}