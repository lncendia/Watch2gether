using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.BaseRoom.Exceptions;
using Overoom.Domain.Rooms.BaseRoom.ValueObject;

namespace Overoom.Domain.Rooms.BaseRoom;

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

    protected void UpdateActivity() => LastActivity = DateTime.UtcNow;

    protected Viewer GetViewer(Guid viewerId)
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