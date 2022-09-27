namespace Watch2gether.WEB.Models.Room.YoutubeRoom;

public class YoutubeRoomViewModel : BaseRoomViewModel
{
    public YoutubeRoomViewModel(List<MessageViewModel> messages, List<ViewerViewModel> viewers, string connectUrl,
        Guid ownerId, ViewerViewModel currentViewer, string url) : base(messages, viewers, connectUrl, ownerId,
        currentViewer)
    {
        Url = url;
    }

    public string Url { get; }
}