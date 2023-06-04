using Overoom.Domain.Abstractions;
using Overoom.Domain.Room.BaseRoom.Exceptions;
using Overoom.Domain.Room.BaseRoom.ValueObject;

namespace Overoom.Domain.Room.BaseRoom.Entities;

public abstract class BaseRoom : AggregateRoot
{
    public bool IsOpen { get; set; } = false;
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;

    protected readonly List<Viewer> ViewersList = new();

    protected readonly List<Message> MessagesList = new();


    public void SetOnline(int viewerId, bool isOnline)
    {
        var viewer = GetViewer(viewerId);
        viewer.Online = isOnline;
        if (!isOnline) viewer.OnPause = true;
        UpdateActivity();
    }

    public void UpdateTimeLine(int viewerId, bool pause, TimeSpan time)
    {
        var viewer = GetViewer(viewerId);
        viewer.OnPause = pause;
        viewer.TimeLine = time;
        UpdateActivity();
    }

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected Viewer GetViewer(int viewerId)
    {
        var viewer = ViewersList.FirstOrDefault(x => x.Id == viewerId);
        if (viewer == null) throw new ViewerNotFoundException();
        return viewer;
    }

    protected void AddViewer(Viewer viewer)
    {
        if (ViewersList.Any(x => x.Id == viewer.Id)) throw new ViewerAlreadyExistsException();
        ViewersList.Add(viewer);
        UpdateActivity();
    }

    protected void AddMessage(Message message)
    {
        if (MessagesList.Count >= 100) MessagesList.RemoveAt(0);
        MessagesList.Add(message);
        UpdateActivity();
    }
}