using Watch2gether.Domain.Rooms.BaseRoom.Entities;
using Watch2gether.Domain.Rooms.BaseRoom.Exceptions;
using Watch2gether.Domain.Rooms.BaseRoom.ValueObject;

namespace Watch2gether.Domain.Rooms.BaseRoom;

public abstract class BaseRoom
{
    protected BaseRoom() => Id = Guid.NewGuid();

    public Guid Id { get; }
    public bool IsOpen { get; set; } = false;
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;
    

    protected readonly List<Viewer> ViewersList = new();

    protected readonly List<Message> MessagesList = new();


    public void SetOnline(Guid viewerId, bool isOnline)
    {
        var viewer = GetViewer(viewerId);
        viewer.Online = isOnline;
        if (!isOnline) viewer.OnPause = true;
        UpdateActivity();
    }

    public void UpdateTimeLine(Guid viewerId, bool pause, TimeSpan time)
    {
        var viewer = GetViewer(viewerId);
        viewer.OnPause = pause;
        viewer.TimeLine = time;
        UpdateActivity();
    }

    public void SendMessage(Guid viewerId, string message)
    {
        SetOnline(viewerId, true);
        MessagesList.Add(new Message(viewerId, message, Id));
        UpdateActivity();
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected Viewer GetViewer(Guid viewerId)
    {
        var viewer = ViewersList.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }
}