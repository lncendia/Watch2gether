namespace Watch2gether.WEB.Models.Room;

public abstract class BaseRoomViewModel
{
    protected BaseRoomViewModel(IEnumerable<MessageViewModel> messages, IEnumerable<ViewerViewModel> viewers,
        string connectUrl, Guid ownerId, Guid currentViewerId)
    {
        Messages = messages.ToList();
        ConnectUrl = connectUrl;
        OwnerId = ownerId;
        Viewers = viewers.ToList();
        CurrentViewer = Viewers.First(x=>x.Id == currentViewerId);
    }

    protected readonly List<MessageViewModel> Messages;
    protected readonly List<ViewerViewModel> Viewers;
    
    public readonly ViewerViewModel CurrentViewer;
    public Guid OwnerId { get; }
    public string ConnectUrl { get; }
}

public abstract class MessageViewModel
{
    protected MessageViewModel(string text, DateTime createdAt, ViewerViewModel viewer)
    {
        Text = text;
        CreatedAt = createdAt.ToLocalTime();
        Viewer = viewer;
    }

    public readonly ViewerViewModel Viewer;
    public DateTime CreatedAt { get; }
    public string Text { get; }
}

public abstract class ViewerViewModel
{
    protected ViewerViewModel(Guid id, string username, string avatarUrl, bool onPause, TimeSpan time)
    {
        Id = id;
        Username = username;
        AvatarFileName = avatarUrl;
        OnPause = onPause;
        Time = time;
    }

    public Guid Id { get; }
    public string AvatarFileName { get; }
    public string Username { get; }
    public bool OnPause { get; }
    public TimeSpan Time { get; }
}