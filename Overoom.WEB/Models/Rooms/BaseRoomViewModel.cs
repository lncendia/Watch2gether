namespace Overoom.WEB.Models.Rooms;

public abstract class BaseRoomViewModel
{
    protected BaseRoomViewModel(IEnumerable<MessageViewModel> messages, IEnumerable<ViewerViewModel> viewers,
        string connectUrl, Guid ownerId, Guid currentViewerId)
    {
        Messages = messages.ToList();
        ConnectUrl = connectUrl;
        OwnerId = ownerId;
        Viewers = viewers.ToList();
        CurrentViewerId = currentViewerId;
    }

    protected readonly List<MessageViewModel> Messages;
    protected readonly List<ViewerViewModel> Viewers;
    
    public Guid CurrentViewerId { get; }
    public Guid OwnerId { get; }
    public string ConnectUrl { get; }
}

public abstract class MessageViewModel
{
    protected MessageViewModel(string text, DateTime createdAt, ViewerViewModel viewer)
    {
        Text = text;
        CreatedAt = createdAt.ToLocalTime().ToString("T");
        Viewer = viewer;
    }

    public readonly ViewerViewModel Viewer;
    public string CreatedAt { get; }
    public string Text { get; }
}

public abstract class ViewerViewModel
{
    protected ViewerViewModel(Guid id, string username, string avatarUri, bool onPause, TimeSpan time)
    {
        Id = id;
        Username = username;
        AvatarUri = avatarUri;
        OnPause = onPause;
        Time = time;
    }

    public Guid Id { get; }
    public string AvatarUri { get; }
    public string Username { get; }
    public bool OnPause { get; }
    public TimeSpan Time { get; }
}