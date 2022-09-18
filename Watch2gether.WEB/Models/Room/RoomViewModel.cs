namespace Watch2gether.WEB.Models.Room;

public class RoomViewModel
{
    public RoomViewModel(List<MessageViewModel> messages, List<ViewerViewModel> viewers, FilmViewModel film,
        string connectUrl, Guid ownerId, ViewerViewModel currentViewer)
    {
        Messages = messages;
        Film = film;
        ConnectUrl = connectUrl;
        OwnerId = ownerId;
        CurrentViewer = currentViewer;
        Viewers = viewers;
    }

    public List<MessageViewModel> Messages { get; }
    public List<ViewerViewModel> Viewers { get; }
    public FilmViewModel Film { get; }
    public ViewerViewModel CurrentViewer { get; }
    public Guid OwnerId { get; }
    public string ConnectUrl { get; }
}

public class MessageViewModel
{
    public MessageViewModel(string text, DateTime createdAt, ViewerViewModel viewer)
    {
        Text = text;
        CreatedAt = createdAt.ToLocalTime();
        Viewer = viewer;
    }

    public ViewerViewModel Viewer { get; }
    public DateTime CreatedAt { get; }
    public string Text { get; }
}

public class ViewerViewModel
{
    public ViewerViewModel(Guid id, string username, string avatarUrl, bool onPause, TimeSpan time)
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

public class FilmViewModel
{
    public FilmViewModel(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }
    public string Url { get; }
}