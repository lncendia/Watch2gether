namespace Overoom.WEB.Models.Rooms;

public abstract class BaseRoomViewModel
{
    protected BaseRoomViewModel(IReadOnlyCollection<MessageViewModel> messages,
        IReadOnlyCollection<ViewerViewModel> viewers,
        string connectUrl, int ownerId, int currentViewerId)
    {
        Messages = messages;
        ConnectUrl = connectUrl;
        OwnerId = ownerId;
        Viewers = viewers;
        CurrentViewerId = currentViewerId;
    }

    public IReadOnlyCollection<MessageViewModel> Messages { get; }
    protected readonly IReadOnlyCollection<ViewerViewModel> Viewers;

    public int CurrentViewerId { get; }
    public int OwnerId { get; }
    public string ConnectUrl { get; }
}

public class MessageViewModel
{
    public MessageViewModel(string text, DateTime createdAt, int viewerId, Uri avatarUri, string username)
    {
        Text = text;
        ViewerId = viewerId;
        AvatarUri = avatarUri;
        Username = username;
        CreatedAt = createdAt.ToLocalTime().ToString("T");
    }

    public string Username { get; }
    public int ViewerId { get; }
    public Uri AvatarUri { get; }
    public string CreatedAt { get; }
    public string Text { get; }
}

public abstract class ViewerViewModel
{
    protected ViewerViewModel(int id, string username, Uri avatarUri, bool onPause, TimeSpan time)
    {
        Id = id;
        Username = username;
        AvatarUri = avatarUri;
        OnPause = onPause;
        Time = time;
    }

    public int Id { get; }
    public Uri AvatarUri { get; }
    public string Username { get; }
    public bool OnPause { get; }
    public TimeSpan Time { get; }
}