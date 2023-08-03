namespace Overoom.WEB.Models.Rooms;

public abstract class ViewerViewModel
{
    protected ViewerViewModel(int id, string username, Uri avatarUri, bool pause, TimeSpan time, bool fullScreen,
        bool allowBeep, bool allowScream, bool allowChange)
    {
        Id = id;
        Username = username;
        AvatarUri = avatarUri;
        Pause = pause;
        Time = time;
        FullScreen = fullScreen;
        AllowBeep = allowBeep;
        AllowScream = allowScream;
        AllowChange = allowChange;
    }

    public int Id { get; }
    public Uri AvatarUri { get; }
    public string Username { get; }
    public bool Pause { get; }
    public bool FullScreen { get; }
    public bool AllowBeep { get; }
    public bool AllowScream { get; }
    public bool AllowChange { get; }
    public TimeSpan Time { get; }
}